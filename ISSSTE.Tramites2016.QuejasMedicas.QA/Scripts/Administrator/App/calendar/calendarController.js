(function() {
    "use strict";

    var controllerId = "calendarController";
    angular
        .module(appName)
        .controller(controllerId, ["common", "calendarDataService", "webApiService", calendarController]);

    function calendarController(common, calendarDataService, webApiService) {
        var vm = this;

        vm.RequestTypeId = null; // Id de tramite
        vm.DelegacionId = null; // Id de delegacion

        vm.Permissions = [];
        vm.delegations = [];

        vm.schedules = [];
        vm.deleteSchedules = [];

        vm.specialDays = [];
        vm.deleteDiaEspecialDTOs = [];

        vm.specialSchedues = [];
        vm.deleteSpecialSchedues = [];

        vm.weekDays = [];
        vm.weekDays.inArray = inArrayFindBy;//Dependencia con scripts.js
        vm.delegationId = -1; // Delegacion
        vm.capacityPerSchedule = 0;

        vm.Schedule = {};
        vm.DiaEspecialDTO = {};
        vm.DiaEspecialDTOSchedule = {};
        vm.Quota = {};
        vm.specialDayScheduleEdit = {};

        vm.initCalendar = initCalendar;
        vm.saveNewSchedule = saveNewSchedule;
        vm.saveNewDiaEspecialDTO = saveNewDiaEspecialDTO;
        vm.saveNewSpecialSchedule = saveNewSpecialSchedule;
        vm.deleteSchedule = deleteSchedule;
        vm.deleteDiaEspecialDTO = deleteDiaEspecialDTO;
        vm.deleteSpecialSchedule = deleteSpecialSchedule;
        vm.delegationChange = delegationChange;
        vm.selectSpecialScheduleDetail = selectSpecialScheduleDetail;
        vm.saveNewQuota = saveNewQuota;
        vm.deleteQuota = deleteQuota;
        vm.formValid = formValid;
        vm.saveCalendar = saveCalendar;
        vm.requestTypeChange = requestTypeChange;
        vm.initFormValidation = initFormValidation;

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);

        }
        
        function initFormValidation(form) {

            if (form == null || form == undefined) return;
            try {
                form.$setPristine();
                form.$setUntouched();
                
            } catch (ex) {
                
            }
        }
        function initCalendar() {
            common.displayLoadingScreen();

            initCalendarData();
            var promises = [];
            promises.push(getRequestTypes());
            promises.push(getWeekdays());

            if (vm.RequestTypeId == null)
                promises.push(getDelegations());

            common.$q.all(promises)
                .finally(function() {
                    init();
                    vm.Permissions = getPermissions();
                    common.hideLoadingScreen();
                });
        }

        function initCalendarData() {
            var today = new Date();

            vm.schedules = [];
            vm.deleteSchedules = [];

            vm.specialDays = [];
            vm.deleteDiaEspecialDTOs = [];

            vm.specialSchedues = [];
            vm.deleteSpecialSchedues = [];

            var stringHour = today.getHours() < 10 ? "0" + String(today.getHours()) : String(today.getHours());
            var stringMinutes = today.getMinutes() < 10 ? "0" + String(today.getMinutes()) : String(today.getMinutes());
            var stringSeconds = today.getSeconds() < 10 ? "0" + String(today.getSeconds()) : String(today.getSeconds());
            var currentTime = stringHour + ":" + stringMinutes + ":" + stringSeconds;

            var stringDay = today.getDate() < 10 ? "0" + String(today.getDate()) : String(today.getDate());
            var stringMonth = today.getMonth() + 1 < 10
                ? "0" + String(today.getMonth() + 1)
                : String(today.getMonth() + 1);
            var stringYear = today.getFullYear() < 10 ? "0" + String(today.getFullYear()) : String(today.getFullYear());
            var currentDate = stringDay + "/" + stringMonth + "/" + stringYear;

            vm.Schedule = {
                ScheduleId: "",
                RequestTypeId: 0,
                DelegationId: 0,
                WeekdayId: null,
                Weekday: "",
                //Time: currentTime,MFP 19-06-2017
                Time: "",//MFP 19-06-2017
                Capacity: "",
                IsNew: false
            };

            vm.DiaEspecialDTO = {
                RequestTypeId: 0,
                DelegationId: 0,
                //Date: currentDate, 19-06-2017
                Date: "",//19-06-2017
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false
            };

            vm.DiaEspecialDTOSchedule = {
                DelegationId: 0,
                RequestTypeId: 0,
                //Date: currentDate, MFP 19-06-2017
                Date:"",
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false,
                Quota: [],
                DeleteQuota: []
            };

            vm.Quota = {
                Id: "",
                //Time: currentTime, 19-06-2017
                Time: "",// 19-06-2017
                Capacity: "",
                IsNew: false
            };
            vm.specialDayScheduleEdit = {};
        }

        function getExistingData() {
            return webApiService.makeRetryRequest(1,
                function() {

                    //if (vm.RequestTypeId == null || vm.delegationId == -1 || vm.delegationId == null) return;

                    common.displayLoadingScreen();
                    return calendarDataService.getExistingData(vm.RequestTypeId, vm.delegationId);
                }).then(function(data) {
                data.Schedules.forEach(function(schedule, index) {
                    vm.schedules.push({
                        ScheduleId: schedule.ScheduleId,
                        DelegationId: schedule.DelegationId,
                        WeekdayId: schedule.WeekdayId,
                        //Weekday: getWeekDayDescription(schedule.WeekdayId),
                        Weekday: vm.weekDays[vm.weekDays.inArray(schedule.WeekdayId, "CatTipoDiaSemanaId")].Dia, //MFP 15-04-2017 
                        Time: schedule.Time,
                        Capacity: schedule.Capacity,
                        IsNew: false
                    });
                });

                data.NonLaborableDays.forEach(function(nonLabourDay, index) {
                    if (nonLabourDay.IsNonWorking) {
                        vm.specialDays.push({
                            DelegationId: nonLabourDay.DelegationId,
                            Date: nonLabourDay.Date,
                            IsNonWorking: nonLabourDay.IsNonWorking,
                            IsOverrided: nonLabourDay.IsOverrided,
                            IsNew: false
                        });
                    }
                });

                data.SpecialSchedules.forEach(function(specialSchedule, index) {
                    var overridenDay = {
                        DelegationId: specialSchedule.DiaEspecialDTO.DelegationId,
                        Date: specialSchedule.DiaEspecialDTO.Date,
                        IsNonWorking: specialSchedule.DiaEspecialDTO.IsNonWorking,
                        IsOverrided: specialSchedule.DiaEspecialDTO.IsOverrided,
                        IsNew: false,
                        Quota: [],
                        NewQuota: [],
                        DeleteQuota: []
                    };

                    specialSchedule.Schedules.forEach(function(currentSchedule, scheduleIndex) {
                        var overridenDayQuota = {
                            Id: currentSchedule.DiaEspecialDTOScheduleId,
                            DelegationId: currentSchedule.DelegationId,
                            Date: currentSchedule.Date,
                            Time: currentSchedule.Time,
                            Capacity: currentSchedule.Capacity,
                            IsNew: false
                        };
                        overridenDay.Quota.push(overridenDayQuota);
                    });

                    vm.specialSchedues.push(overridenDay);
                });
                common.hideLoadingScreen();
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetCalendarData);
                common.hideLoadingScreen();
            });
        }


        function getRequestTypes() {

            return webApiService.makeRetryRequest(1,
                    function() {
                        return calendarDataService.getRequestTypes();
                    })
                .then(function(data) {
                    vm.requestTypes = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequestTypes);
                });
        }

        function getUserDelegationsByConfig() {
            return webApiService.makeRetryRequest(1,
                function() {
                    common.displayLoadingScreen();
                    return calendarDataService.getUserDelegationsByConfig(vm.RequestTypeId);
                }).then(function(data) {
                vm.delegations = data;
                if (data.length == 1) {
                    vm.delegationId = vm.delegations[0].DelegacionId;
                    getExistingData();
                }
                common.hideLoadingScreen();
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetDelegations);
                common.hideLoadingScreen();
            });
        }

        function getDelegations() {
            return webApiService.makeRetryRequest(1,
                function() {
                    return calendarDataService.getUserDelegations();
                }).then(function(data) {
                vm.delegations = data;
                if (data.length == 1) {
                    vm.delegationId = vm.delegations[0].DelegacionId;
                    getExistingData();
                }
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetDelegations);
            });
        }

        function requestTypeChange() {

            if (vm.RequestTypeId == null) {
                initCalendarData(); //MFP initdata
            } else {
                initCalendarData(); //MFP initdata
                getUserDelegationsByConfig();
            }
            vm.delegationId = null;
        }

        function delegationChange() {

            //if (vm.delegationId == null) return;
            initCalendarData(); ////MFP initdata
            getExistingData();
        }

        function saveNewSchedule() {
            var error = validateNewSchedule();
            if (error != "") {
                common.showErrorMessage(error);
                return;
            }

            vm.schedules.push({
                ScheduleId: vm.ScheduleId,
                DelegationId: vm.delegationId,
                RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                WeekdayId: vm.Schedule.WeekdayId,
                //Weekday: getWeekDayDescription(vm.Schedule.WeekdayId),
                Weekday: vm.weekDays[vm.weekDays.inArray(vm.Schedule.WeekdayId, "CatTipoDiaSemanaId")]
                    .Dia, //MFP 15-04-2017 
                Time: getTimeFromStringEditor(vm.Schedule.Time),
                Capacity: vm.Schedule.Capacity,
                IsNew: true
            });

            vm.Schedule = {
                ScheduleId: "",
                DelegationId: 0,
                WeekdayId: 0,
                Time: "",
                Capacity: 0,
                IsNew: false
            };

            common.showSuccessMessage(Messages.success.hourItemAdded);
        }

        function saveNewDiaEspecialDTO() {
            var error = validateNewDiaEspecialDTO();
            if (error != "") {
                //console.log('error en el día especial');
                common.showErrorMessage(error);
                return;
            }

            vm.specialDays.push({
                DelegationId: vm.delegationId,
                RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                Date: vm.DiaEspecialDTO.Date,
                IsNonWorking: true,
                IsOverrided: false,
                IsNew: true
            });

            vm.DiaEspecialDTO = {
                DelegationId: 0,
                Date: new Date(),
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false
            };

            common.showSuccessMessage(Messages.success.nonLaboralDayAdded);
        }

        function saveNewSpecialSchedule() {
            var error = validateNewSpecialSchedule();
            if (error != "") {
                common.showErrorMessage(error);
                return;
            }

            vm.specialSchedues.push({
                DelegationId: vm.delegationId,
                RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                Date: vm.DiaEspecialDTOSchedule.Date,
                IsNonWorking: false,
                IsOverrided: true,
                IsNew: true,
                Quota: [],
                DeleteQuota: []
            });

            vm.DiaEspecialDTOSchedule = {
                DelegationId: 0,
                RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                Date: new Date(),
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false,
                Quota: [],
                DeleteQuota: []
            };

            common.showSuccessMessage(Messages.success.specialDayAdded);
        }

        function saveNewQuota() {
            var error = validateNewQuota();
            if (error != "") {
                common.showErrorMessage(error);
                return;
            }

            vm.Quota.IsNew = true;
            vm.Quota.Time = getTimeFromStringEditor(vm.Quota.Time);
            vm.Quota.RequestTypeId = vm.RequestTypeId; // MFP 27-03-2017

            vm.specialDayScheduleEdit.Quota.push(vm.Quota);


            vm.Quota = {
                Time: "",
                Capacity: 0,
                IsNew: false
            };

            common.showSuccessMessage(Messages.success.hourItemAdded);
        }

        function deleteSchedule(schedule) {
            if (!schedule.IsNew) {
                vm.deleteSchedules.push(schedule);
            }

            var index = vm.schedules.indexOf(schedule);
            if (index > -1) {
                vm.schedules.splice(index, 1);
            }
        }

        function deleteDiaEspecialDTO(specialDay) {
            if (!specialDay.IsNew) {
                vm.deleteDiaEspecialDTOs.push(specialDay);
            }

            var index = vm.specialDays.indexOf(specialDay);
            if (index > -1) {
                vm.specialDays.splice(index, 1);
            }
        }

        function deleteSpecialSchedule(specialSchedule) {
            if (!specialSchedule.IsNew) {
                vm.deleteSpecialSchedues.push(specialSchedule);
            }

            var index = vm.specialSchedues.indexOf(specialSchedule);
            if (index > -1) {
                vm.specialSchedues.splice(index, 1);
            }
        }

        function deleteQuota(quota) {
            if (!quota.IsNew) {
                vm.specialDayScheduleEdit.DeleteQuota.push(quota);
                for (var x = 0; x <= vm.specialDayScheduleEdit.Quota.length - 1; x++) {
                    if (vm.specialDayScheduleEdit.Quota[x].Id == quota.Id) {
                        vm.specialDayScheduleEdit.Quota.splice(x, 1);
                        calendarDataService
                            .getDeleteHorarioDiaEspecialDTOs(vm.RequestTypeId, quota.DelegationId, quota.Time)
                            .success(function(data, status, headers, config) {
                                common.showSuccessMessage(Messages.success.deleteHorarioDiaEspecialDTOs);
                            })
                            .error(function(data, status, headers, config) {
                                common.showErrorMessage(Messages.error.errorDeleteHorarioDiaEspecialDTOs);
                            });
                        break;
                    }
                }
            }

            var index = vm.specialDayScheduleEdit.Quota.indexOf(quota);
            if (index > -1) {
                vm.specialDayScheduleEdit.Quota.splice(index, 1);
            }
        }

        function validateNewSchedule() {
            var error = "";

            if (vm.Schedule.Capacity < 1 || vm.Schedule.WeekdayId < 1) {
                error = Messages.error.errorCalendarSchedule;
            }

            vm.Schedule.Time = getTimeFromStringEditor(vm.Schedule.Time);

            if (error == "") {
                vm.schedules.forEach(function(currentSchedule, index) {
                    if (currentSchedule.WeekdayId == vm.Schedule.WeekdayId &&
                        currentSchedule.Time == vm.Schedule.Time) {
                        error = Messages.error.errorCalendarScheduleAndDayRepet;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewDiaEspecialDTO() {
            var error = "";
            var today = new Date();
            var selectedStringDate = getDateFromStringEditor(vm.DiaEspecialDTO.Date);
            console.log('Fecha seleccionada');
            console.log(selectedStringDate);
            if (selectedStringDate < today) {
                error = Messages.error.errorCalendarSchedule;
            }

            if (error == "") {
                vm.specialDays.forEach(function(currentDiaEspecialDTO, index) {
                    if (vm.DiaEspecialDTO.Date == currentDiaEspecialDTO.Date) {
                        error = Messages.error.errorCalendarDayRepet;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewSpecialSchedule() {
            var error = "";
            var today = new Date();
            var selectedStringDate = getDateFromStringEditor(vm.DiaEspecialDTOSchedule.Date);
            if (selectedStringDate < today) {
                error = Messages.error.errorCalendarSchedule;
            }

            if (error == "") {
                vm.specialSchedues.forEach(function(currentspecialSchedule, index) {
                    if (vm.DiaEspecialDTOSchedule.Date == currentspecialSchedule.Date) {
                        error = Messages.error.errorCalendarDayRepet;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewQuota() {
            var error = "";

            if (vm.specialDayScheduleEdit.Quota == null) {
                error = "Selecciona una fecha";
            }

            if (error == "" && vm.Quota.Capacity < 1) {
                error = Messages.error.errorCalendarSchedule;
            }

            vm.Quota.Time = getTimeFromStringEditor(vm.Quota.Time);
            if (error == "") {
                vm.specialDayScheduleEdit.Quota.forEach(function(currentQuota, index) {
                    if (vm.Quota.Time == currentQuota.Time) {
                        error = Messages.error.errorCalendarScheduleRepet;
                        return false;
                    }
                });
            }

            return error;
        }

        function saveCalendar(delegationId) {
            try{ // GFB 21/06/2017
                common.displayLoadingScreen();
                saveDiaEspecialDTOs().then(function() {
                    saveNonLabourDays().then(function() {
                        saveSchedules().then(function() {
                            common.hideLoadingScreen();
                            //common.showSuccessMessage(Messages.success.itemUpdated);
                            delegationChange();
                        },
                            function(reason) {
                                common.showErrorMessage(Messages.error.errorSaveCalendar);
                                common.hideLoadingScreen();
                            });
                    },
                        function(reason) {
                            common.showErrorMessage(Messages.error.errorSaveCalendar);
                            common.hideLoadingScreen();
                        });

                },
                    function(reason) {
                        common.showErrorMessage(reason, Messages.error.errorSaveCalendar);
                        common.hideLoadingScreen();
                        return;
                    });
                common.showSuccessMessage(Messages.success.itemUpdated);
            }
            catch (ex) {
                common.showErrorMessage(ex);
                common.hideLoadingScreen();
            }
        }

        //function weekdayChange() {
        //    vm.Schedule.WeeydayDescription = getWeekDayDescription(vm.Schedule.WeekdayId);
        //}

        function selectSpecialScheduleDetail(specialDay) {
            vm.specialDayScheduleEdit = specialDay;
        }

        function formValid(form) {
            return form.$valid;
        }

        function getDateFromStringEditor(editorDate) {
            //editorDate DD/MM/YYYY
            // verificar que la fecha no venga vacía GFB 21/06/2017
            var newDate;
            if (editorDate.trim() != "") {
                var dateParts = editorDate.split("/");
                var stringDate = dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2];
                var milliseconds = Date.parse(stringDate);
                newDate = new Date(milliseconds);
            }
            else {
                newDate = Date.parse("01/01/0001");
            }
            return newDate;
        }

        function getTimeFromStringEditor(time) {
            var timeParts = time.split(":");
            var hour = Number(timeParts[0]);
            var minutes = Number(timeParts[1].substring(0, 2));
            var amPmParts = timeParts[1].split(" ");
            if (amPmParts[1] == "PM") {
                hour += 12;
                if (hour == 24)
                    hour = 12;

            }
            var stringHour = hour < 10 ? "0" + String(hour) : String(hour);
            var stringMinutes = minutes < 10 ? "0" + String(minutes) : String(minutes);

            var newDate = stringHour + ":" + stringMinutes + ":00";
            return newDate;
        }

        function getWeekdays() {
            calendarDataService.getWeekdays()
                .success(function(data) {
                    vm.weekDays = data;
                    vm.weekDays.inArray = inArrayFindBy;
                })
                .error(function(data) {
                    common.showErrorMessage(data, Messages.error.errorGetWeekdays);
                });
        }


        function saveSchedules() {
            var promises = [];

            //Elimina los horarios normales
            vm.deleteSchedules.forEach(function(actualSchedule, index) {
                promises.push(calendarDataService.deleteSchedule(vm.RequestTypeId,
                    actualSchedule.DelegationId,
                    actualSchedule.Time,
                    actualSchedule.WeekdayId));
            });

            var newSchedules = [];
            vm.schedules.forEach(function(currentSchedule, index) {
                if (currentSchedule.IsNew) {
                    //newSchedule.Time = getTimeFromStringEditor(newSchedule.Time);
                    newSchedules.push(currentSchedule);
                }
            });

            promises.push(calendarDataService.saveSchedules(newSchedules));
            return common.$q.all(promises);
        }

        function saveNonLabourDays() {
            var promises = [];
            vm.deleteDiaEspecialDTOs.forEach(function(nonLabourDay, index) {
                promises.push(calendarDataService.deleteDiaEspecialDTO({
                        DelegationId: nonLabourDay.DelegationId,
                        Date: nonLabourDay.Date,
                        IsNonWorking: nonLabourDay.IsNonWorking // MFP 27-03-2017
                    })
                    .success(function(data, status, headers, config) {
                        //common.showSuccessMessage(Messages.success.itemUpdated);
                    })
                    .error(function(data, status, headers, config) {
                        common.showErrorMessage(data);
                    }));
            });

            var newDiaEspecialDTOs = [];
            vm.specialDays.forEach(function(currentDiaEspecialDTO, index) {
                if (currentDiaEspecialDTO.IsNew) {
                    newDiaEspecialDTOs.push(currentDiaEspecialDTO);
                }
            });

            for (var x = 0; x <= newDiaEspecialDTOs.length - 1; x++) {
                for (var y = 0; y <= vm.specialSchedues.length - 1; y++) {
                    if (newDiaEspecialDTOs[x].Date == vm.specialSchedues[y].Date) {
                        common.showErrorMessage(Messages.error.errorCalendarDaysSpecial);
                        newDiaEspecialDTOs.splice(x, 1);
                        for (var x = 0; x <= newDiaEspecialDTOs.length - 1; x++) {
                            for (var y = 0; y <= vm.specialSchedues.length - 1; y++) {
                                if (newDiaEspecialDTOs[x].Date == vm.specialSchedues[y].Date) {
                                    common.showErrorMessage(Messages.error.errorCalendarDaysSpecial);
                                    newDiaEspecialDTOs.splice(x, 1);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            if (newDiaEspecialDTOs.length > 0) {
                promises.push(calendarDataService.saveDiaEspecialDTOs(newDiaEspecialDTOs)
                    .success(function(data) {
                        //common.showSuccessMessage(Messages.success.itemUpdated);
                    })
                    .error(function(data) {
                        var count = vm.specialDays.length - 1;
                        for (var x = 0; x <= count; x++) {
                            if (vm.specialDays[x].Date == newDiaEspecialDTOs[0].Date) {
                                vm.specialDays.splice(x, 1);
                            }
                        }

                    }));
            }
            return common.$q.all(promises);
        }

        function saveDiaEspecialDTOs() {
            var promises = [];
            var deleteSpecialScheduesAndHour = [];
            
                vm.deleteSpecialSchedues.forEach(function(specialSchedule, index) {
                    if (specialSchedule.Quota.length > 0) {
                        specialSchedule.Quota.forEach(function(quota, indexQuota) {
                            if (quota.Id != "") {
                                promises.push(calendarDataService.deleteSpecialScheduleDays(quota.Id)
                                    .success(function(data, status, headers, config) {
                                    })
                                    .error(function(data, status, headers, config) {
                                        common.showErrorMessage(data);
                                    }));
                                deleteSpecialScheduesAndHour.push(quota);
                            }
                        });
                    }

                    promises.push(calendarDataService.deleteDiaEspecialDTO({
                        DelegationId: specialSchedule.DelegationId,
                        IsNonWorking: specialSchedule.IsNonWorking, // Add MFP 27-03-2017
                        Date: specialSchedule.Date
                    }));
                });

                var newSpecialSchedues = [];
                var newQuotas = [];


                vm.specialSchedues.forEach(function(currentSpecialSchedule, index) {
                    if (currentSpecialSchedule.IsNew) {
                        newSpecialSchedues.push(currentSpecialSchedule);
                    } else {
                        currentSpecialSchedule.Quota.forEach(function(currentQuota, quotaIndex) {
                            if (currentQuota.IsNew) {
                                newQuotas.push({
                                    DiaEspecialDTOScheduleId: "",
                                    DelegationId: vm.delegationId,
                                    RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                                    Date: currentSpecialSchedule.Date,
                                    Time: getTimeFromStringEditor(currentQuota.Time),
                                    Capacity: currentQuota.Capacity
                                });
                            }
                        });
                    }
                });

                for (var x = 0; x <= newSpecialSchedues.length - 1; x++) {
                    for (var y = 0; y <= vm.specialDays.length - 1; y++) {
                        if (newSpecialSchedues[x].Date == vm.specialDays[y].Date) {
                            //common.showErrorMessage(Messages.error.errorCalendarDaysNoWorking);
                        
                            newSpecialSchedues.splice(x, 1);
                            for (var x = 0; x <= newSpecialSchedues.length - 1; x++) {
                                for (var y = 0; y <= vm.specialDays.length - 1; y++) {
                                    if (newSpecialSchedues[x].Date == vm.specialDays[y].Date) {
                                        //common.showErrorMessage(Messages.error.errorCalendarDaysNoWorking);
                                        newSpecialSchedues.splice(x, 1);
                                        console.log('salí de la función de guardado de los días especiales');
                                        throw (Messages.error.errorCalendarDaysNoWorking);
                                    }
                                }
                            }
                            console.log('salí de la función de guardado de los días especiales');
                            throw (Messages.error.errorCalendarDaysNoWorking);
                        }
                    }
                }

                //Se guardan los dias especiales
                if (newSpecialSchedues.length > 0) {
                    promises.push(calendarDataService.saveDiaEspecialDTOs(newSpecialSchedues)
                        .success(function(data) {
                            newSpecialSchedues.forEach(function(newSpecialSchedue, index) {
                                var quotas = [];
                                newSpecialSchedue.Quota.forEach(function(currentQuota, quotaIndex) {
                                    quotas.push({
                                        DiaEspecialDTOScheduleId: "",
                                        DelegationId: vm.delegationId,
                                        RequestTypeId: vm.RequestTypeId, // MFP 27-03-2017
                                        Date: newSpecialSchedue.Date,
                                        Time: getTimeFromStringEditor(currentQuota.Time),
                                        Capacity: currentQuota.Capacity
                                    });
                                });

                                if (quotas.length > 0)
                                    promises.push(calendarDataService.saveSpecialScheduleDays(quotas));

                            });

                            //common.showSuccessMessage(Messages.success.itemUpdated);
                        })
                        .error(function(data) {
                            var count = vm.specialSchedues.length - 1;
                            for (var x = 0; x <= count; x++) {
                                if (vm.specialSchedues[x].Date == newSpecialSchedues[0].Date) {
                                    vm.specialSchedues.splice(x, 1);
                                }
                            }
                        }));

                }

                if (newQuotas.length > 0)
                    promises.push(calendarDataService.saveSpecialScheduleDays(newQuotas));


                return common.$q.all(promises);
            
            
        }

        function getPermissions() {
            var Permissions = [];

            angular.forEach(Constants.roles,
                function(item) {

                    if (common.doesUserHasNecessaryRoles([item.names])) {
                        Permissions = item.Permissions;
                    }

                });

            return Permissions;

        }

    }
})();