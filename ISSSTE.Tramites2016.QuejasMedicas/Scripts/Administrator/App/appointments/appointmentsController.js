(function () {
    "use strict";

    var controllerId = "appointmentsController";
    angular
        .module(appName)
        .controller(controllerId, ["common", "appointmentsDataService", "webApiService", appointmentsController]);

    function appointmentsController(common, appointmentsDataService, webApiService) {
        var vm = this;

        vm.delegations = [];
        vm.delegationId = null;
        vm.RequestTypeId = null;
        var d = new Date();
        vm.startDate = d.toLocaleDateString("en-GB");
        vm.endDate = d.toLocaleDateString("en-GB");
        vm.dates = vm.startDate + "-" + vm.endDate;
        vm.selectedStatus = null;
        vm.query = "";


        vm.appointments = []; // Informacion de las citas
        vm.statusList = [];
        vm.requestTypes = [];



        vm.init = init;
        vm.initSearchApp = initSearchApp;
        vm.searchAppointments = searchAppointments;
        vm.selectStatus = selectStatus;
        vm.clearAllFilters = clearAllFilters;

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function clearAllFilters() {

            if (vm.delegations.length > 1) {
                vm.delegationId = null;
            }
            vm.RequestTypeId = null;
            vm.startDate = null;
            vm.endDate = null;
            vm.selectedStatus = null;
            setStatusValues(0, "Todos los estatus");
            vm.query = "";
        }

        function isValidFilters() {

            if ((vm.delegationId == null || vm.delegations.length == 1) &&
                vm.RequestTypeId == null &&
                vm.selectedStatus == null &&
                vm.query == "" &&
                (vm.startDate == null ||
                vm.endDate == null)) {
                common.showErrorMessage(Messages.validation.filtersRequiered);
                return false;
            }

            if (!(isValidDate(vm.startDate) && isValidDate(vm.endDate)) && !(vm.startDate == null || vm.endDate == null)) {
                common.showErrorMessage(Messages.validation.invalidDate);
                return false;
            }

            return true;

        }

        function initSearchApp() {

            common.displayLoadingScreen();
            var statusListPromise = getStatusList();
            var requestsPromise = getAppointments();
            var requestTypesPromise = getRequestTypes();
            var delegationsPromise = getDelegations();

            common.$q.all([requestTypesPromise, delegationsPromise, statusListPromise, requestsPromise])
                .finally(function () {
                    init();
                    UI.initStatusDropdown();
                    common.hideLoadingScreen();
                });
        }


        function getDelegations() {
            return webApiService.makeRetryRequest(1,
                function () {
                    return appointmentsDataService.getUserDelegations();
                }).then(function (data) {
                    vm.delegations = data;
                    if (data.length == 1) {
                        vm.delegationId = vm.delegations[0].DelegacionId;
                    }
                }).catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetDelegations);
                });
        }


        function selectStatus(status) {
            vm.selectedStatus = status;
            //vm.searchAppointments();
        }


        function searchAppointments() {

            if (!isValidFilters()) { return; }

            vm.dates = vm.startDate + "-" + vm.endDate;
            if (vm.dates == null || vm.dates == "") return;

            common.displayLoadingScreen();

            getAppointments()
                .finally((function () {
                    common.hideLoadingScreen();
                }));
        }


        function getAppointments() {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return appointmentsDataService.getAppointments(vm.dates,
                            vm.query,
                            vm.selectedStatus != null ? vm.selectedStatus.EstadoCitaId : null,
                            vm.delegationId,
                            vm.RequestTypeId);
                    })
                .then(function (data) {
                    InitCalendar(data.ListAppointments);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetAppointmentData);
                });
        }


        function getRequestTypes() {

            return webApiService.makeRetryRequest(1,
                    function () {
                        return appointmentsDataService.getRequestTypes();
                    })
                .then(function (data) {
                    vm.requestTypes = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequestTypes);
                });
        }


        function InitCalendar(ListAppointments) {


            function replacer(key, value) {

                if (typeof value === "" || value === null) {
                    return undefined;
                }

                return value;
            }

            var stringJSON = JSON.stringify(ListAppointments, replacer)
                .replace(/ColorTramite/g, "colortramite")
                .replace(/NumeroFolio/g, "numerofolio")
                .replace(/Class/g, "class")
                .replace(/Start/g, "start")
                .replace(/Title/g, "title")
                .replace(/Url/g, "url")
                .replace(/Id/g, "id")
                .replace(/End/g, "end");

            var appointmentsJSON = JSON.parse(stringJSON);

            vm.appointments = appointmentsJSON;

            Date.prototype.yyyymmdd = function () {
                var mm = this.getMonth() + 1;
                var dd = this.getDate();

                return [
                    this.getFullYear(),
                    (mm > 9 ? "" : "0") + mm,
                    (dd > 9 ? "" : "0") + dd
                ].join("-");
            };

            var date = new Date();
            var urlTemplates = common.getBaseUrl() + "Administrador/";

            var options = {
                events_source: function () {
                    return appointmentsJSON;
                },
                view: "month",
                tmpl_path: urlTemplates,
                tmpl_cache: false,
                day: date.yyyymmdd(),
                modal: "#events-modal",
                modal_type: "ajax",
                language: "es-MX",
                onAfterEventsLoad: function (events) {
                    if (!events) {
                        return;
                    }
                    //var list = $('#eventlist');
                    //list.html('');

                    //$.each(events, function (key, val) {
                    //    $(document.createElement('li'))
                    //        .html('<a href="' + val.url + '">' + val.title + '</a>')
                    //        .appendTo(list);
                    //});
                },
                onAfterViewLoad: function (view) {

                    $("#page-header").text(this.getTitle());
                    $(".btn-group button").removeClass("active");
                    $('button[data-calendar-view="' + view + '"]').addClass("active");
                },
                classes: {
                    months: {
                        general: "label"
                    }
                }
            };

            var calendar = $("#calendar").calendar(options);

            $(".form-group button[data-calendar-nav]").each(function () {
                var $this = $(this);
                $this.click(function () {
                    calendar.navigate($this.data("calendar-nav"));
                });
            });

            $(".btn-group button[data-calendar-view]").each(function () {
                var $this = $(this);
                $this.click(function () {
                    calendar.view($this.data("calendar-view"));
                });
            });

            $("#first_day").change(function () {
                var value = $(this).val();
                value = value.length ? parseInt(value) : null;
                calendar.setOptions({ first_day: value });
                calendar.view();
            });

            //$('#events-in-modal').change(function () {
            //    var val = $(this).is(':checked') ? $(this).val() : null;

            //    calendar.setOptions({ modal: val });
            //});

            $("#events-modal .modal-header, #events-modal .modal-footer").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
            });


        }

        function getStatusList() {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return appointmentsDataService.getStatusList();
                    })
                .then(function (data) {
                    vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                });
        }


    }
})();