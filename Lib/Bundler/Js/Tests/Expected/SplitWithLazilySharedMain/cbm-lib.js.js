!function(e) {
    "use strict";
    (function(n, t) {
        var o = __bbb[t];
        return o !== e ? o instanceof Promise ? o : Promise.resolve(o) : __bbb[t] = new Promise(function(e, o) {
            var r = document.createElement("script");
            r.type = "text/javascript", r.charset = "utf-8", r.onload = function() {
                e(__bbb[t]);
            }, r.onerror = function(e) {
                o("Failed to load " + n);
            }, r.src = n, document.head.appendChild(r);
        });
    })(e, "b").then(function(e) {
        return e.shared();
    });
    function n() {
        return "Hello";
    }
    __bbb.a = {
        hello: n
    };
}();