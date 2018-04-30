/*@cc_on@if(@_jscript_version<5.6)ns_={Form:function(){}};ns_.Form.onFailure=function(){};ns_.Form.prototype.onFailure=function(){};@else@*/
if (typeof (ns_) == "undefined") {
    ns_ = new Object();
}
ns_.extend = function(a, c) {
    for (var b in c) {
        a[b] = c[b];
    }
    return a;
};
ns_.encode = self.encodeURIComponent
    ? self.encodeURIComponent
    : function(a) {
        return escape(a).replace(/\//g, "%2F");
    };
ns_.b = {};
ns_.b.ie =
    /*@cc_on!@*/
    false;
ns_.b.ie9 = ns_.b.ie && document.addEventListener;
ns_.b.sf = navigator.vendor ? /apple/i.test(navigator.vendor) : false;
ns_.b.ch = window.chrome ? true : false;
ns_.b.ff = /firefox/i.test(navigator.userAgent);
ns_.b.op = window.opera ? true : false;
if (typeof (ns_) == "undefined") {
    ns_ = new Object();
}
ns_.dt = {
    initDate: new Date().getTime(),
    lastRun: new Date().getTime(),
    delta: 0,
    intervalID: null,
    intervalTime: 1000,
    intervalRuns: 0,
    timedObserver: function() {
        var a = ns_.dt.getTime();
        ns_.dt.intervalID = setTimeout(ns_.dt.timedObserver, ns_.dt.intervalTime);
        ns_.dt.intervalRuns++;
        var b = a - ns_.dt.lastRun - ns_.dt.intervalTime;
        if (b > (ns_.dt.intervalTime * 2) || b < -(ns_.dt.intervalTime * 2)) {
            ns_.dt.delta += b;
            a -= b;
        }
        ns_.dt.lastRun = a;
    },
    getTime: function() {
        var a = new Date().getTime();
        return a - ns_.dt.delta;
    }
};
ns_.dt.timedObserver();
ns_.extend(Function.prototype,
    (function() {
        var e = Array.prototype.slice;

        function d(k, h) {
            var j = k.length,
                i = h.length;
            while (i--) {
                k[j + i] = h[i];
            }
            return k;
        }

        function f(i, h) {
            i = e.call(i, 0);
            return d(i, h);
        }

        function a() {
            return this.STargumentIsSuper() ? ["$super"] : [""];
        }

        function c() {
            return /^[\s\(]*function[^(]*\(\s*\$super/.test(this.toString());
        }

        function b(j) {
            if (arguments.length < 2 && typeof arguments[0] == "undefined") {
                return this;
            }
            var h = this,
                i = e.call(arguments, 1);
            return function() {
                var k = f(i, arguments);
                return h.apply(j, k);
            };
        }

        function g(i) {
            var h = this;
            return function() {
                var j = d([h.STbind(this)], arguments);
                return i.apply(this, j);
            };
        }

        return {
            STargumentNames: a,
            STargumentIsSuper: c,
            STbind: b,
            STwrap: g
        };
    })());
ns_.Class = {
    extend: function(c, a) {
        if (arguments.length == 1) {
            a = c, c = null;
        }
        if (typeof (c) == "function") {
            var b = function() {
                return this.initialize.apply(this, arguments);
            };
            b.prototype = new c();
        } else {
            var b = function() {};
        }
        if (a) {
            ns_.Class.inherit(b.prototype, a);
        }
        return b;
    },
    inherit: function(e, b, h) {
        if (arguments.length == 3) {
            var c = e[h],
                d = b[h],
                g = d;
            var a;
            if (e && g.STargumentIsSuper()) {
                d = (function(f) {
                    return function() {
                        return f.apply(this, arguments);
                    };
                })(c).STwrap(g);
            } else {
                d = function() {
                    var j = this.parent;
                    this.parent = c;
                    var f = g.apply(this, arguments);
                    j ? this.parent = j : delete this.parent;
                    return f;
                };
            }
            d.valueOf = function() {
                return g;
            };
            d.toString = function() {
                return g.toString();
            };
            e[h] = d;
        } else {
            for (var i in b) {
                if (e[i] && typeof (b[i]) == "function") {
                    ns_.Class.inherit(e, b, i);
                } else {
                    e[i] = b[i];
                }
            }
        }
        return e;
    }
};
if (typeof (ns_.ar) == "undefined") {
    ns_.ar = {
        push: function() {
            for (var b = 0, a = arguments.length; b < a; b++) {
                this[this.length] = arguments[b];
            }
        },
        splice: function() {
            var j = ns_.ar.create(),
                d = arguments;
            if (d.length <= 1) {
                return j;
            } else {
                if (d.length > 2) {
                    for (var f = 2, c = d.length; f < c; f++) {
                        j.push(d[f]);
                    }
                }
            }
            var b = this.slice(0, d[0]);
            var e = this.slice(d[0], d[0] + d[1]);
            var h = this.slice(d[0] + d[1]);
            var g = b.concat(j, h);
            this.length = 0;
            for (var f = 0, c = g.length; f < c; f++) {
                this.push(g[f]);
            }
            return e;
        },
        shift: function() {
            for (var d = 0, a = this[0], c = this.length - 1; d < c; d++) {
                this[d] = this[d + 1];
            }
            this.length--;
            return a;
        }
    };
}
ns_.ar.create = function() {
    var c = [];
    for (var a in c) {
        delete c[a];
    }
    for (var d = 0, b = arguments.length; d < b; d++) {
        c[c.length] = arguments[d];
    }
    return c;
};
ns_.ar.indexOf = function(c, b) {
    var a = -1;
    ns_.ar.each(c,
        function(e, d) {
            if (e == b) {
                a = d;
            }
        },
        this);
    return a;
};
ns_.ar.merge = function() {
    var a = ns_.ar.create();
    ns_.ar.each(arguments,
        function(b) {
            ns_.ar.each(b,
                function(d, c) {
                    a[c] = d;
                });
        },
        this);
    return a;
};
ns_.ar.pair = function(c, b) {
    var a = ns_.ar.create();
    ns_.ar.each(b,
        function(e, d) {
            a.push(d + c + e);
        },
        this);
    return a;
};
ns_.ar.each = function(g, f, d) {
    try {
        if (typeof (f) == "function") {
            d = typeof (d) != "undefined" ? d : this.each.caller;
            if (typeof g.length != "number" || typeof g[0] == "undefined") {
                var b = typeof (g.__proto__) != "undefined";
                for (var c in g) {
                    if ((!b || (b && typeof (g.__proto__[c]) == "undefined")) && typeof g[c] != "function") {
                        f.call(d, g[c], c);
                    }
                }
            } else {
                for (var c = 0, a = g.length; c < a; c++) {
                    f.call(d, g[c], c);
                }
            }
        }
    } catch (h) {
    }
};
ns_.st = {
    camel: function(e, d) {
        if (!d) {
            d = " ";
        }
        var b = e.split(d);
        for (var c = 0, a = b.length; c < a; c++) {
            b[c] = b[c].charAt(0).toUpperCase() + b[c].substr(1).toLowerCase();
        }
        return b.join("");
    }
};
if (typeof (ns_.dom) == "undefined") {
    ns_.dom = {
        cache: {},
        getElementsByTagName: function(b, c) {
            var a,
                b = b,
                c = document.getElementById(c) || document;
            if (c.getElementsByTagName) {
                a = c.getElementsByTagName(b);
            } else {
                if (c.all && c.all.tags) {
                    a = c.all.tags(b);
                }
            }
            return a || [];
        },
        addEvent: (document.addEventListener)
            ? (function(a, c, b) {
                a.addEventListener(c, b, false);
                if (ns_.dom.cache) {
                    ns_.dom.cache.add(a, c, b, false);
                }
                return true;
            })
            : (document.attachEvent)
            ? (function(a, c, b) {
                var d;
                if (typeof a.addEventListener != "undefined") {
                    d = a.addEventListener(c, b);
                } else {
                    c = (c.substring(0, 1) == c.substring(0, 1).toUpperCase() ? c : "on" + c);
                    d = a.attachEvent(c, b);
                }
                if (d && ns_.dom.cache) {
                    ns_.dom.cache.add(a, c, b, false);
                }
                return d;
            })
            : (function(c, b, a) {
                if (document.layers && c == document && b.toLowerCase() == "click") {
                    b = "mouseup";
                    c.captureEvents(Event.MOUSEUP);
                }
                var d = c["on" + b];
                if (typeof d != "function") {
                    c["on" + b] = a;
                } else {
                    c["on" + b] = function(f) {
                        if (d) {
                            d(f);
                        }
                        a(f);
                    };
                }
                if (ns_.dom.cache) {
                    ns_.dom.cache.add(c, b, a, false);
                }
                return true;
            }),
        removeEvent: function(a, c, b) {
            ns_.dom.cache.del(a, c, b);
        }
    };
}
ns_.dom.cache = {
    _events: ns_.ar.create(),
    add: function(b, d, c, a) {
        this._events.push(arguments);
    },
    del: function(a, c, b) {
        ns_.ar.each(this._events,
            function(e, d) {
                if (!!!b && e[0] == a && e[1] == c) {
                    this.detach(e[0], e[1], e[2], e[3]);
                    this._events.splice(d, 1);
                    throw "Event found";
                } else {
                    if (!!b && e[0] == a && e[1] == c && e[2] == b) {
                        this.detach(e[0], e[1], e[2], e[3]);
                        this._events.splice(d, 1);
                        throw "Event found";
                    }
                }
            },
            ns_.dom.cache);
    },
    detach: function(b, d, c, a) {
        if (typeof b.removeEventListener != "undefined") {
            if (ns_.b.ie) {
                b.removeEventListener(d, c);
            } else {
                b.removeEventListener(d, c, a);
            }
        } else {
            if (b.detachEvent) {
                b.detachEvent(d, c);
            }
        }
    },
    flush: function() {
        try {
            ns_.ar.each(this._events,
                function(c, b) {
                    if (typeof c != "undefined") {
                        this.detach(c[0], c[1], c[2], c[3]);
                        this._events.splice(b, 1);
                    }
                },
                ns_.dom.cache);
        } catch (a) {
        }
    }
};
ns_.dom.addEvent(window, "unload", ns_.dom.cache.flush.STbind(ns_.dom.cache));
ns_.Error = {};
ns_.Class.extend((function() {
    var c = false;

    function e(g) {
        c = !!g;
    }

    function f(g) {
        var h = [new Date()];
        h = h.concat([
            ("0" + h[0].getHours()).slice(-2), ("0" + h[0].getMinutes()).slice(-2), ("0" + h[0].getSeconds()).slice(-2),
            ("00" + h[0].getMilliseconds()).slice(-3)
        ]);
        g = f.caller[0] = "[" + h[1] + ":" + h[2] + ":" + h[3] + "." + h[4] + "] " + g;
        return true;
    }

    function b(j) {
        for (var h = 0, g = arguments.length; h < g; h++) {
            f(arguments[h]);
        }
        a();
    }

    function d(j) {
        for (var h = 0, g = arguments.length; h < g; h++) {
            f(arguments[h]);
        }
        a();
        if (c) {
            throw j;
        }
        return false;
    }

    function a() {
        if (c && typeof (console) == "object") {
            if (console[a.caller.name]) {
                console[a.caller.name].apply(console, a.caller.arguments);
            } else {
                if (console.log) {
                    for (var h = 0, g = a.caller.arguments.length; h < g; h++) {
                        console.log(a.caller.arguments[h]);
                    }
                }
            }
        } else {
            if (c && (!a.caller.name || a.caller.name == "error")) {
                for (var h = 0, g = a.caller.arguments.length; h < g; h++) {
                    alert(a.caller.arguments[h]);
                }
            }
        }
    }

    ns_.Class.inherit(ns_.Error,
        {
            log: b,
            debug: b,
            error: d,
            setDebug: e
        });
})());
ns_.Lifecycle = ns_.Class.extend(function() {},
    {
        log: ns_.Error.error,
        current: null,
        states: null,
        params: null,
        initialize: function(a, b) {
            if (typeof a != "string" || a.length < 1) {
                this.log("ns_.Lifecycle: EmptyStates");
            }
            if (typeof b != "function") {
                this.log("ns_.Lifecycle: InvalidStartCallback");
            }
            a = a.split(" ");
            ns_.ar.each(a,
                function(d, c) {
                    if (!/^\w+$/i.test(d)) {
                        a.splice(c, 1);
                    }
                },
                this);
            if (a.length < 1) {
                this.log("ns_.Lifecycle: InvalidStates");
            }
            this.states = a;
            this.current = "";
            this.params = {};
        },
        property: function(a, b, c) {
            if (!a) {
                this.log("ns_.Lifecycle: EmptyName");
            }
            this["get" + ns_.st.camel(a)] = function() {
                return this.params[a];
            };
            if (!b) {
                this["set" + ns_.st.camel(a)] = function(d) {
                    this.params[a] = d;
                };
            } else {
                this["set" + ns_.st.camel(a)] = b;
            }
            if (c) {
                this["set" + ns_.st.camel(a)].call(this, c);
            } else {
                this["set" + ns_.st.camel(a)].call(this, "");
            }
        },
        doChecks: function(b, a) {
            if (!(this.states && this.states.length > 0)) {
                throw "ns_.Lifecycle: EmptyStates";
            }
            if (ns_.ar.indexOf(this.states, b) == -1) {
                throw "ns_.Lifecycle: InvalidMutation";
            }
            if ((typeof a != "function") && (typeof a != "object")) {
                throw "ns_.Lifecycle: InvalidEntitlement";
            }
        },
        creation: function(b, d, f, a) {
            try {
                this.doChecks(f, a);
            } catch (c) {
                this.log(c);
            }
            ns_.ar.each(d,
                function(g, e) {
                    this.property(e, g);
                },
                this);
            this[b] = (function(k, l, g, i) {
                i = i || {};
                var h = ns_.ar.create();
                ns_.ar.each(k,
                    function(n, m) {
                        if (typeof (i[m]) != "undefined") {
                            try {
                                this["set" + ns_.st.camel(m)].call(this, i[m]);
                            } catch (o) {
                                h.push(o);
                            }
                        }
                    },
                    this);
                if (this.current != "") {
                    h.push("ns_.Lifecycle: InvalidSourceState");
                }
                if (!h.length) {
                    try {
                        g.apply(this);
                        this.current = String(l);
                        ns_.Error.log("ns_.Lifecycle: Created at '" + l + "'");
                    } catch (j) {
                        this.log(j);
                        return false;
                    }
                    return true;
                }
                return false;
            }).STbind(this, d, f, a);
        },
        transition: function(b, d, g, f, a) {
            try {
                this.doChecks(f, a);
            } catch (c) {
                this.log(c);
            }
            ns_.ar.each(d,
                function(h, e) {
                    this.property(e, h);
                },
                this);
            this[b] = (function(l, n, m, h, j) {
                j = j || {};
                var i = ns_.ar.create();
                ns_.ar.each(l,
                    function(p, o) {
                        if (typeof (j[o]) != "undefined") {
                            try {
                                this["set" + ns_.st.camel(o)].call(this, j[o]);
                            } catch (q) {
                                i.push(q);
                            }
                        }
                    },
                    this);
                if (ns_.ar.indexOf(n.split(" "), this.current) < 0) {
                    i.push("ns_.Lifecycle: InvalidSourceState");
                }
                if (!i.length) {
                    try {
                        h.apply(this);
                        this.current = String(m);
                        ns_.Error.log("ns_.Lifecycle: Transitioning to '" + m + "'");
                    } catch (k) {
                        this.log(k);
                        return false;
                    }
                    return true;
                }
                return false;
            }).STbind(this, d, g, f, a);
        }
    });
ns_.Form = ns_.Class.extend(function() {},
    {
        VERSION: "2.1204.27",
        conf: {
            DEBUG: false,
            TIMER_DELAY: 500,
            LABEL_NS: "ns_fo_",
            INIT_TIMER: 30000,
            SUBMIT_TIMER: 1000,
            FAILURE_TIMER: 1000,
            pixelUrl: "",
            labels: {}
        },
        init: null,
        start: null,
        id: null,
        tag: null,
        fields: null,
        settings: null,
        lastField: null,
        validations: null,
        lastValidated: null,
        lastValidation: null,
        sequence: null,
        state: null,
        wasSubmitted: null,
        watcher: null,
        fix: null,
        fail: null,
        button: null,
        pixelUrl: function() {
            var c = this.conf.pixelUrl ||
                (self.ns_p && typeof ns_p.src == "string" ? ns_p.src : (self.ns_pixelUrl ? ns_pixelUrl : ""));
            var f = c.indexOf("&");
            if (f != -1) {
                var b = c.substring(0, f),
                    e = c.substr(f + 1).split("&");
                for (var d = 0, a = e.length; d < a; d++) {
                    if (/(c[23456]|name)/.test(e[d].split("=")[0])) {
                        b += "&" + e[d];
                    }
                }
                return b;
            } else {
                if (c.indexOf("?") == -1 || c.charAt(c.length - 1) == "?") {
                    return c + ((c.charAt(c.length - 1) != "?") ? "?" : "") + this.id;
                }
            }
            return c;
        },
        initialize: function(g, l, e) {
            this.init = ns_.dt.getTime();
            var c = (function() {
                this.state = new ns_.Lifecycle("inactive active failed complete abandoned", function() {});
                this.state.property("form", null, this);
                this.state.creation("onInit",
                    {},
                    "inactive",
                    function() {
                        try {
                            var n = this.params.form;
                            n.start = 0;
                            n.validations = ns_.ar.create();
                            n.wasSubmitted = false;
                            n.watch();
                            if (!self.ns_p && !self.ns_pixelUrl && !n.conf.pixelUrl) {
                                ns_.Error.log("ns_.Form: Attention, no pixelUrl found");
                            }
                        } catch (o) {
                            ns_.Error.error(o);
                        }
                    });
                var f = {
                    lastField: null,
                    type: null
                };
                this.state.transition("onInput",
                    f,
                    "active inactive failed",
                    "active",
                    function() {
                        try {
                            var n = this.params.lastField,
                                o = this.params.type,
                                p = this.params.form;
                            if (!p.start) {
                                p.start = ns_.dt.getTime();
                            }
                            if (n != null && n != undefined) {
                                if (n.getType()) {
                                    p.lastField = n;
                                }
                                n.value();
                            }
                            if (o && o != "blur") {
                                p.countDown();
                            }
                        } catch (q) {
                            ns_.Error.error(q);
                        }
                    });
                this.state.transition("onError",
                    {
                        field: null,
                        message: null
                    },
                    "inactive active failed complete abandoned",
                    "failed",
                    function() {
                        try {
                            var p = this.params.form;

                            if (!p.shallTransmit("failure")) {
                                return;
                            }
                            var o = p.field(this.params.field);
                            if (o.length > 0) {
                                o = o[0];
                            } else {
                                throw "ns_.Form.onFailure(): unrecognized field [" + this.params.field + "]";
                            }
                            var r = ns_.ar.create(o.id, this.params.message);
                            p.validations.push(r);
                            p.lastValidated = o;
                            p.lastValidation = this.params.message;
                            if (p.wasSubmitted && p.wasSubmitted.type == ns_.Form.SUBMIT) {
                                p.sequence--;
                                p.wasSubmitted = false;
                            }
                            p.fix = undefined;
                            p.fail = ns_.dt.getTime();
                            var n = new ns_.Form.Measurement(p, ns_.Form.FAILURE);
                            ns_.Form.sitestat(p.pixelUrl(), n.labels);
                            p.values();
                        } catch (q) {
                            ns_.Error.error(q);
                        }
                    });
                this.state.transition("onSubmit",
                    {},
                    "inactive active failed",
                    "complete",
                    function() {
                        try {
                            var p = this.params.form,
                                r = ns_.dt.getTime();
                            if (!p.shallTransmit("submit")) {
                                return;
                            }
                            if (!isNaN(p.fail) && r - p.fail < p.conf.FAILURE_TIMER) {
                                throw "ns_.Form: Canceled submit colliding with failures";
                            } else {
                                p.fail = undefined;
                            }
                            var o = new ns_.Form.Measurement(p, ns_.Form.SUBMIT);
                            p.wasSubmitted = o;
                        } catch (q) {
                            ns_.Error.error(q);
                        }
                    });
                this.state.transition("onAbandon",
                    {},
                    "active failed",
                    "abandoned",
                    function() {
                        try {
                            var o = this.params.form;
                            if (!o.shallTransmit("abandon")) {
                                return;
                            }
                            var n = new ns_.Form.Measurement(o, ns_.Form.ABANDON);
                            ns_.Form.sitestat(o.pixelUrl(), n.labels);
                        } catch (p) {
                            ns_.Error.error(p);
                        }
                    });
            }).STbind(this);
            l = l || {};
            e = e || {};
            ns_.ar.each(this.conf,
                function(n, f) {
                    this.conf[f] = (typeof (e[f]) != "undefined") ? e[f] : n;
                },
                this);
            ns_.Error.setDebug(this.conf.DEBUG);
            var k = "ns_.Form(): '" + g + "' was provided, but no form was found";
            if (!g || g == "") {
                ns_.Error.error("ns_.Form(): argument formName is blank. Provide it or an asterisk * for all");
                return new Boolean(0);
            } else {
                if (typeof g == "object") {
                    var i = ns_.Form.get(g);
                    if (i) {
                        return i;
                    }
                    this.tag = g;
                    this.id = g = this.fetchID(this.tag);
                } else {
                    if (g == "*") {
                        var h = ns_.dom.getElementsByTagName("FORM");
                        if (h.length > 0) {
                            this.id = this.fetchID(h[0]);
                            this.tag = h[0];
                            ns_.ar.each(h,
                                function(f) {
                                    new ns_.Form(f, l, e);
                                },
                                this);
                            if (ns_.Form.get(this.id)) {
                                return ns_.Form.get(this.id);
                            }
                        } else {
                            if (!ns_.Form.loaded) {
                                ns_.Form.cache.push(ns_.ar.create(g, l, e));
                            } else {
                                ns_.Error.error(k);
                            }
                            return new Boolean(0);
                        }
                    } else {
                        if (typeof g == "string") {
                            if (!ns_.Form.get(g)) {
                                var d = this.form(g);
                                if (d) {
                                    this.tag = d;
                                    this.id = this.fetchID(this.tag);
                                } else {
                                    if (!ns_.Form.loaded) {
                                        ns_.Form.cache.push(ns_.ar.create(g, l, e));
                                    } else {
                                        ns_.Error.error(k);
                                    }
                                    return new Boolean(0);
                                }
                            } else {
                                return ns_.Form.get(g);
                            }
                        }
                    }
                }
            }
            this.sequence = 1;
            this.fail = undefined;
            this.fix = undefined;
            this.watcher = 0;
            this.id = this.id || "";
            this.fields = ns_.ar.create();
            this.anchors = ns_.ar.create();
            if (typeof this.tag == "undefined") {
                return new Boolean(0);
            }
            ns_.Form.store.push(this);
            this.settings = this.defaults();
            var b,
                j,
                m = ns_.Form.Element,
                a = this.defaults;
            ns_.ar.each(this.tag.elements,
                function(f) {
                    new m(f, this, a(false));
                },
                this);
            ns_.ar.each(ns_.dom.getElementsByTagName("A", this.tag),
                function(f) {
                    new m(f, this, a(false));
                },
                this);
            this.parseFields(l);
            c();
            this.isVisible = this.isVisible.STbind(this);
            ns_.dom.addEvent(this.tag,
                "submit",
                (function() {
                    this.state.onSubmit();
                }).STbind(this));
            ns_.dom.addEvent(this.tag,
                "mousedown",
                (function() {
                    this.state.onInput({
                        lastField: null,
                        type: null
                    });
                }).STbind(this));
            this.state.onInit();
        },
        parseFields: function(a) {
            var b = ns_.Form.Element,
                c = this.defaults;
            if (typeof a != "undefined" && typeof a == "object") {
                ns_.ar.each(a,
                    function(e, d) {
                        if (d == "*" || (d == 0 && e == "*")) {
                            this.settings = c(a["*"]);
                            ns_.ar.each(this.tag.elements,
                                function(f) {
                                    new b(f, this, c(this.settings));
                                },
                                this);
                        } else {
                            if (d != "") {
                                switch (d) {
                                case "abandon":
                                    this.settings = c(a);
                                    break;
                                case "submit":
                                    this.settings = c(a);
                                    break;
                                case "failure":
                                    this.settings = c(a);
                                    break;
                                case "password":
                                    this.settings = c(a);
                                    break;
                                case "hidden":
                                    this.settings = c(a);
                                    break;
                                default:
                                    ns_.ar.each(this.element(d),
                                        function(f) {
                                            new b(f, this, c(a[d]));
                                        },
                                        this);
                                    break;
                                }
                            }
                        }
                    },
                    this);
            } else {
                if (a == "*") {
                    this.settings = c();
                    ns_.ar.each(this.tag.elements,
                        function(d) {
                            new b(d, this, c(this.settings));
                        },
                        this);
                }
            }
        },
        watch: function() {
            if (this.watcher == 0 && (this.state.current == "" || this.state.current == "inactive")) {
                this.watcher = setTimeout((function(a) {
                        return function() {
                            if (a.current == "inactive" && a.params.form.isVisible() && ns_.Form.wfocus) {
                                a.onInput({
                                    lastField: null,
                                    type: null
                                });
                            }
                            a.params.form.watcher = 0;
                        };
                    })(this.state),
                    this.conf.INIT_TIMER);
            }
        },
        unwatch: function() {
            if (!isNaN(this.watcher)) {
                clearTimeout(this.watcher);
                this.watcher = 0;
            }
        },
        defaults: function(b) {
            if (typeof conf == "undefined") {
                conf = null;
            } else {
                if (!b) {
                    b = false;
                }
            }
            var a = ns_.extend({
                    submit: true,
                    abandon: false,
                    failure: false,
                    password: {
                        submit: false,
                        abandon: false,
                        failure: false
                    },
                    hidden: {
                        submit: false,
                        abandon: false,
                        failure: false
                    }
                },
                this.settings);
            if (b != null && typeof b == "object") {
                ns_.ar.each(b,
                    function(d, c) {
                        if (typeof d != "undefined") {
                            switch (c) {
                            case "submit":
                            case "abandon":
                            case "failure":
                                a[c] = !!d;
                                break;
                            case "password":
                            case "hidden":
                                if (d != null && typeof d == "object") {
                                    ns_.ar.each(d,
                                        function(f, e) {
                                            if (typeof f != "undefined") {
                                                switch (e) {
                                                case "submit":
                                                case "abandon":
                                                case "failure":
                                                    a[c][e] = !!f;
                                                    break;
                                                default:
                                                    break;
                                                }
                                            }
                                        },
                                        this);
                                } else {
                                    if (d == true) {
                                        a[c] = {
                                            submit: true,
                                            abandon: true,
                                            failure: true
                                        };
                                    }
                                }
                            default:
                                break;
                            }
                        }
                    },
                    this);
            } else {
                if (b == false) {
                    a = {
                        submit: false,
                        abandon: false,
                        failure: false,
                        hidden: {
                            submit: false,
                            abandon: false,
                            failure: false
                        },
                        password: {
                            submit: false,
                            abandon: false,
                            failure: false
                        }
                    };
                }
            }
            return a;
        },
        shallTransmit: function(b) {
            var a = false;
            try {
                if (this.settings[b] || this.settings.hidden[b] || this.settings.password[b]) {
                    a = true;
                }
            } catch (c) {
            }
            return a;
        },
        form: function(b) {
            var a, c, b = b || "";
            c = ns_.dom.getElementsByTagName("FORM");
            ns_.ar.each(c,
                function(d) {
                    if (d.name == b || d.id == b) {
                        a = d;
                    }
                },
                this);
            return a;
        },
        element: function(b) {
            var a = ns_.ar.create(),
                b = b || "";
            ns_.ar.each(this.tag.elements,
                function(c) {
                    if (c.name == b || c.id == b) {
                        a.push(c);
                    }
                },
                this);
            return a;
        },
        field: function(b) {
            var a = ns_.ar.create(),
                b = b || "";
            ns_.ar.each(this.fields,
                function(c) {
                    if (c.id == b) {
                        a.push(c);
                    }
                },
                this);
            return a;
        },
        fetchID: function(a) {
            if (a.name != "" && typeof a.name == "string") {
                return a.name;
            } else {
                if (a.id != "" && typeof a.id == "string") {
                    return a.id;
                } else {
                    a.id = "form" + ns_.ar.indexOf(document.forms, a);
                    return a.id;
                }
            }
        },
        registerField: function(c) {
            var b = ns_.ar.create();
            ns_.ar.each(this.fields,
                function(d) {
                    if (d.id == c.id && (d.tag.type || "").toLowerCase() != "radio") {
                        b.push(d);
                    }
                },
                this);
            if (!b.length) {
                this.fields.push(c);
                ns_.dom.addEvent(c.tag, "keyup", c.onUserAction);
                ns_.dom.addEvent(c.tag, "mousedown", c.onUserAction);
                ns_.dom.addEvent(c.tag, "change", c.onUserAction);
                ns_.dom.addEvent(c.tag, "blur", c.onUserAction);
                ns_.dom.addEvent(c.tag, "focus", c.onUserAction);
                var a = c.tag.type;
                if (a == "submit" || a == "button" || a == "image") {
                    this.button = true;
                }
            } else {
                ns_.ar.each(b,
                    function(d) {
                        d.setup(c.settings);
                    },
                    this);
                delete c;
            }
        },
        values: function() {
            var a = ns_.ar.create();
            ns_.ar.each(this.fields,
                function(b) {
                    a[b.id] = b.value();
                },
                this);
            return a;
        },
        onUnload: function() {
            if (this.wasSubmitted == false &&
                !this.button &&
                (!isNaN(this.fix) && ns_.dt.getTime() - this.fix < this.conf.SUBMIT_TIMER)) {
                this.fix = undefined;
                this.state.onSubmit();
            }
            if (!this.wasSubmitted) {
                this.state.onAbandon();
            } else {
                ns_.Form.sitestat(this.pixelUrl(), this.wasSubmitted.labels);
                this.wasSubmitted = false;
            }
        },
        countDown: function() {
            if (!this.button) {
                this.fix = ns_.dt.getTime();
            }
        },
        onFailure: function(b, a) {
            this.state.onError({
                field: b,
                message: a
            });
        },
        style: function(b) {
            var a = "";
            if (document.defaultView && document.defaultView.getComputedStyle) {
                a = document.defaultView.getComputedStyle(this.tag, "").getPropertyValue(b);
            } else {
                if (this.tag.currentStyle) {
                    b = b.replace(/\-(\w)/g,
                        function(c, d) {
                            return d.toUpperCase();
                        });
                    a = this.tag.currentStyle[b];
                }
            }
            return a;
        },
        isVisible: function() {
            var b = this.style("display"),
                a = this.style("visibility");
            if (b == "none" || a == "hidden") {
                return false;
            }
            return true;
        },
        beat: function() {
            var c = false,
                b = document.getElementsByName(this.id),
                d = document.getElementById(this.id) || b[0];
            if (b.length < 1 && !d) {
                c = true;
            } else {
                if (!this.isVisible()) {
                    c = true;
                }
            }
            if (c) {
                this.onUnload();
            }
        }
    });
ns_.Form.SUBMIT = "submit";
ns_.Form.ABANDON = "submitabandon";
ns_.Form.FAILURE = "submitvalfail";
ns_.Form.start = typeof (ns_loadingtime1) != "undefined" ? ns_loadingtime1 : ns_.dt.getTime();
ns_.Form.rx = {
    TypeLb: /type/i,
    AllForms: /\*/,
    FormElm: /^(input|select|textarea|a|button)$/i,
    InputElm: /^(input|select|textarea)$/i,
    InputElmTy: /[^(submit|reset|button)]/i,
    Input: /^input$/i,
    Hidden: /^inputhidden$/i,
    Password: /^inputpassword$/i,
    Radio: /^inputradio$/i,
    Checkbox: /^inputcheckbox$/i,
    TextArea: /^textarea$/i,
    SelectBox: /^select$/i,
    Failure: /^submitvalfail$/i,
    TLDrx: /^(https?\:)?(\/\/)?[^\/\?]+[\/\?]/i
};
ns_.Form.store = ns_.ar.create();
ns_.Form.cache = ns_.ar.create();
ns_.Form.loaded = false;
ns_.Form.get = function(b) {
    var a, b = b || 0;
    ns_.ar.each(ns_.Form.store,
        function(d, c) {
            if (typeof (b) == "string") {
                if (d.id == b) {
                    a = d;
                }
            } else {
                if (typeof (b) == "object") {
                    if (d == b || d.tag == b) {
                        a = d;
                    }
                } else {
                    if (!isNaN(b)) {
                        if (b == c) {
                            a = d;
                        }
                    }
                }
            }
        },
        this);
    return a;
};
ns_.Form.beat = function() {
    ns_.ar.each(ns_.Form.store,
        function(a) {
            a.beat();
        },
        this);
};
if (ns_.b.ff || ns_.b.sf) {
    ns_.dom.addEvent(document,
        "DOMSubtreeModified",
        function() {
            ns_.Form.beat();
        });
}
ns_.dom.addEvent(document, "mousedown", ns_.Form.beat);
ns_.Form.onFailure = function(b, d, a) {
    if (b == null || b == "") {
        ns_.ar.each(ns_.Form.store,
            function(e) {
                e.onFailure(d, a);
            },
            this);
    } else {
        var c = ns_.Form.get(b);
        if (c != null) {
            c.onFailure(d, a);
        }
    }
};
ns_.Form.onLoad = function(g) {
    var d = ns_.Form;
    d.loaded = true;
    if (!d.store.length && d.cache.length) {
        for (var c = 0, a = d.cache.length; c < a; c++) {
            var g = d.cache[c];
            new d(g[0], g[1], g[2]);
        }
    } else {
        if (!d.store.length) {
            var b = function(i, f, e) {
                ns_.ar.each(f,
                    function(j) {
                        var k = j.className.split(" ");
                        ns_.ar.each(k,
                            function(l) {
                                if (l == i) {
                                    e(j);
                                }
                            },
                            this);
                    },
                    this);
            };
            var h = ns_.dom.getElementsByTagName("FORM");
            if (h.length > 0) {
                b("ns_",
                    h,
                    function(e) {
                        new ns_.Form(e);
                    });
                ns_.ar.each(ns_.Form.store,
                    function(i) {
                        var e = {},
                            f = {
                                abandon: true,
                                failure: true
                            };
                        b("ns_",
                            i.tag.elements,
                            function(j) {
                                e[j.id || j.name] = f;
                            });
                        i.parseFields(e);
                        i.settings = i.defaults(f);
                    },
                    this);
            }
        }
    }
};
ns_.Form.onUnload = function(a) {
    ns_.ar.each(ns_.Form.store,
        function(b) {
            b.onUnload();
        },
        this);
};
ns_.dom.addEvent(window, "beforeunload", ns_.Form.onUnload);
ns_.dom.addEvent(window, "load", ns_.Form.onLoad);
ns_.dom.addEvent(window, "unload", ns_.Form.onUnload);
ns_.Form.wfocus = true;
ns_.dom.addEvent(window,
    ns_.ie ? "focusout" : "blur",
    function(a) {
        ns_.Form.wfocus = false;
        ns_.ar.each(ns_.Form.store,
            function(b) {
                b.unwatch();
            });
    });
ns_.dom.addEvent(window,
    ns_.ie ? "focusin" : "focus",
    function(a) {
        ns_.Form.wfocus = true;
        ns_.ar.each(ns_.Form.store,
            function(b) {
                b.watch();
            });
    });
ns_.Form.sitestat = function(b, g) {
    var e = window.sitestat =
        ((typeof (window.sitestat) == "function") &&
            (!ns_.b.ie && g && (typeof (g.ns_fo_ev) == "undefined" || ns_.Form.rx.Failure.test(g.ns_fo_ev))))
        ? window.sitestat
        : function(j) {
            var m = new Image();
            m.src = j;
            if (!ns_.b.ie) {
                for (var k = 0, h = 100; k < h; k++) {
                    var n = new Function("a", "b", "return a+b");
                    var o = n(k, 1);
                }
            }
        };
    if (b) {
        g = (typeof g == "object" ? g : {});
        var f = b;
        ns_.ar.each(g,
            function(i, h) {
                f = f + "&" + h + "=" + i;
            },
            this);
        if (ns_.Form.rx.TLDrx.test(b)) {
            e(f);
        } else {
            var a = "",
                c;
            if (self.ns_p && typeof ns_p.src == "string") {
                a = ns_p.src;
            } else {
                if (self.ns_pixelUrl) {
                    a = ns_pixelUrl;
                }
            }
            if (a) {
                c = a.lastIndexOf("?");
                a = a.substring(0, c > 0 ? c : a.length) + "?" + f;
            }
            if (a) {
                var d = a.indexOf("&");
                if ((d > 0) && !ns_.Form.rx.TypeLb.test(a)) {
                    e(a.substring(0, d) + "&type=hidden" + a.substring(d));
                } else {
                    if (!ns_.Form.rx.TypeLb.test(a)) {
                        e(a + "&type=hidden");
                    } else {
                        e(a);
                    }
                }
            }
        }
    }
};
ns_.Form.Element = ns_.Class.extend(function() {},
    {
        form: null,
        id: null,
        tag: null,
        settings: null,
        cache: null,
        throttle: null,
        skipKeys: ns_.ar.create(),
        initialize: function(d, f, e) {
            var c = this;
            if (!ns_.Form.rx.FormElm.test(d.tagName)) {
                return new Boolean(false);
            }
            c.tag = d;
            c.form = f;
            c.id = c.fetchID();
            c.cache = "";
            c.throttle = ns_.ar.create();
            if (!c.skipKeys.length) {
                for (var b = 1, a = 94; b < a; b++) {
                    c.skipKeys.push(b);
                }
                for (var b = 112, a = 146; b < a; b++) {
                    c.skipKeys.push(b);
                }
                c.skipKeys.push(224);
            }
            c.onDispatch = c.onDispatch.STbind(this);
            c.onUserAction = c.onUserAction.STbind(this);
            c.settings = {
                submit: true,
                abandon: false,
                failure: false
            };
            c.setup(e);
            c.value();
            c.form.registerField(this);
        },
        setup: function(c) {
            var b = ns_.Form.rx,
                a = this.tag;
            if (!b.InputElm.test(a.tagName)) {
                c = this.form.defaults(false);
            } else {
                if (b.Hidden.test(a.tagName + a.type)) {
                    if (c && typeof c.hidden == "object") {
                        c = c.hidden;
                    }
                } else {
                    if (b.Password.test(a.tagName + a.type)) {
                        if (c && typeof c.password == "object") {
                            c = c.password;
                        }
                    }
                }
            }
            ns_.ar.each(c,
                function(e, d) {
                    switch (d) {
                    case "submit":
                    case "abandon":
                    case "failure":
                        this.settings[d] = e;
                        break;
                    case "password":
                    case "hidden":
                    default:
                        break;
                    }
                },
                this);
        },
        fetchID: function() {
            var a = this.tag;
            if (a.name && a.name != "") {
                return a.name;
            } else {
                if (a.id && a.id != "") {
                    return a.id;
                } else {
                    a.id = "input" + ns_.ar.indexOf(this.form.tag.elements, a);
                    return a.id;
                }
            }
        },
        value: function() {
            var a = "",
                f = this.getType(),
                g = ns_.Form.rx,
                e = this.tag;
            if (f == ns_.Form.Element.PASSWD) {
                a = e.value == "" ? "no" : "yes";
            } else {
                if (g.Checkbox.test(e.tagName + e.type)) {
                    if (e.checked) {
                        a = "yes";
                    } else {
                        a = "no";
                    }
                } else {
                    if (g.Input.test(e.tagName) ||
                        g.TextArea.test(e.tagName) ||
                        g.Radio.test(e.tagName + e.type) ||
                        g.Hidden.test(e.tagName + e.type)) {
                        a = e.value;
                    } else {
                        if (g.SelectBox.test(e.tagName)) {
                            if (e.multiple) {
                                var c = ns_.ar.create();
                                for (var d = 0, b = e.options.length; d < b; d++) {
                                    if (e.options[d].selected) {
                                        c.push(e.options[d].value);
                                    }
                                }
                                a = c;
                            } else {
                                if (e.selectedIndex != -1) {
                                    a = e.options[e.selectedIndex].value;
                                }
                            }
                        }
                    }
                }
            }
            this.cache = a;
            return a;
        },
        getType: function() {
            var a = this.tag.tagName + this.tag.type,
                b = ns_.Form.rx;
            if (b.Hidden.test(a)) {
                return ns_.Form.Element.HIDDEN;
            } else {
                if (b.Password.test(a)) {
                    return ns_.Form.Element.PASSWD;
                } else {
                    if (b.InputElm.test(this.tag.tagName) &&
                        (b.Input.test(this.tag.tagName) ? b.InputElmTy.test(this.tag.type) : true)) {
                        return ns_.Form.Element.NORMAL;
                    } else {
                        return false;
                    }
                }
            }
        },
        onUserAction: function(c) {
            var c = c || window.event,
                b = this,
                h = c.type || "all",
                g = true,
                a = c.which || c.keyCode,
                f = ns_.dt.getTime();
            if (ns_.ar.indexOf(this.skipKeys, String.fromCharCode(a)) != -1) {
                g = false;
            }
            if (g) {
                var d = {
                    lastField: b,
                    type: h
                };
                b.form.state.onInput(d);
                if (h == "blur") {
                    b.form.beat();
                }
                b.throttle[h] = f;
            }
        },
        onDispatch: function(d, a) {
            if (this.getType() == false ||
                (ns_.Form.rx.Radio.test(this.tag.tagName + this.tag.type) && !this.tag.checked)) {
                return false;
            }
            var e = this.form.conf.LABEL_NS + this.getType(),
                c = this.settings;
            var f = false;
            switch (d) {
            case ns_.Form.SUBMIT:
                f = c.submit;
                break;
            case ns_.Form.ABANDON:
                f = c.abandon;
                break;
            case ns_.Form.FAILURE:
                f = c.failure;
                break;
            }
            if (f) {
                var b = typeof (this.cache) == "string" ? ns_.ar.create(this.cache) : this.cache;
                ns_.ar.each(b,
                    function(g) {
                        if (typeof (a[e + "n"]) == "string") {
                            a[e + "n"] = a[e + "n"] + ";" + ns_.encode(this.id);
                            a[e + "v"] = a[e + "v"] + ";" + ns_.encode(g);
                        } else {
                            a[e + "n"] = ns_.encode(this.id);
                            a[e + "v"] = ns_.encode(g);
                        }
                    },
                    this);
            }
        }
    });
ns_.Form.Element.NORMAL = "f";
ns_.Form.Element.PASSWD = "p";
ns_.Form.Element.HIDDEN = "h";
ns_.Form.Measurement = ns_.Class.extend(function() {},
    {
        form: null,
        type: ns_.Form.SUBMIT,
        labels: null,
        initialize: function(b, a) {
            this.labels = ns_.extend(ns_.ar.create(), b.conf.labels);
            this.form = b;
            this.setType(a);
            this.dispatch();
        },
        setType: function(a) {
            switch (a) {
            case ns_.Form.ABANDON:
            case ns_.Form.FAILURE:
            case ns_.Form.SUBMIT:
                this.type = a;
                break;
            }
        },
        resetLabels: function() {
            this.labels = ns_.ar.create();
        },
        setLabels: function(c) {
            if (typeof (c) == "string") {
                var a = ns_.ar.merge(ns_.ar.create(), c.split("&")),
                    b = ns_.ar.create();
                ns_.ar.each(a,
                    function(d, e) {
                        if (d.indexOf("=")) {
                            var f = d.split("=");
                            b[f[0]] = f[1];
                        }
                    },
                    this);
                this.labels = ns_.ar.merge(this.labels, b);
            } else {
                if (typeof c.pop != "null") {
                    this.labels = ns_.ar.merge(this.labels, c);
                }
            }
        },
        dispatch: function() {
            var d = ns_.dt.getTime(),
                g = this.form,
                e = g.conf.LABEL_NS,
                c = this.labels;
            c.type = "hidden";
            c[e + "ev"] = this.type;
            c[e + "id"] = ns_.encode(g.id);
            c[e + "la"] = g.lastField != null ? ns_.encode(g.lastField.id) : "";
            c[e + "sq"] = g.sequence++;
            c[e + "t0"] = d - ns_.Form.start;
            c[e + "t1"] = d - g.start;
            if (g.validations.length > 0) {
                var b = ns_.ar.create();
                ns_.ar.each(g.validations,
                    function(f, h) {
                        if (typeof (b[f[0]]) == "number") {
                            b[f[0]]++;
                        } else {
                            b[f[0]] = 1;
                        }
                    },
                    this);
                var a = ns_.ar.create();
                ns_.ar.each(b,
                    function(f, h) {
                        a.push(ns_.encode(h) + ":" + f);
                    },
                    this);
                c[e + "vfo"] = a.join("|");
                c[e + "vf"] = g.validations.length;
            }
            if (g.lastValidated != null) {
                c[e + "vfl"] = ns_.encode(g.lastValidated.id);
            }
            if (typeof (g.lastValidation) == "string" && g.lastValidation != "") {
                c[e + "vfe"] = ns_.encode(g.lastValidation.substring(0, 255));
            }
            ns_.ar.each(g.fields,
                function(i, f) {
                    try {
                        i.onDispatch(this.type, c);
                    } catch (h) {
                        ns_.Error.error(h);
                    }
                },
                this);
            c[e + "sv"] = g.VERSION;
        }
    });
/*@end @*/