var __bbb={};!function(n) {
    "use strict";
    var e = function(e, t) {
        var o = __bbb[t];
        return o !== n ? o instanceof Promise ? o : Promise.resolve(o) : __bbb[t] = new Promise(function(n, o) {
            var r = document.createElement("script");
            r.type = "text/javascript", r.charset = "utf-8", r.onload = function() {
                n(__bbb[t]);
            }, r.onerror = function(n) {
                o("Failed to load " + e);
            }, r.src = e, document.head.appendChild(r);
        });
    };
    e("shared.js", "a").then(function() {
        return e("lib.js", "b");
    }).then(function(n) {
        console.log(n.hello());
    }), e("shared.js", "a").then(function() {
        return e("lib2.js", "c");
    }).then(function(n) {
        console.log(n.world());
    });
}();