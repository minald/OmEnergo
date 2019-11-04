// Must be recompiled manually after every change due to the bug in the WebOptimizer tool,
// when js or jsx files cannot be auto-recompiled, i.e. rebundled and reminified, on saving.
// This comment can be removed after resolving the 'https://github.com/madskristensen/WebCompiler/issues/402' issue

'use strict';

// For the navigation tree sidebar
$('.fa-angle-right').click(function () {
    $(this).toggleClass('fa-angle-right');
    $(this).toggleClass('fa-angle-down');
});

// The Jivosite widget
(function () {
    var widget_id = 'r8B1qiUPl0';
    var d = document;
    var w = window;
    function l() {
        var s = document.createElement('script');
        s.type = 'text/javascript';
        s.async = true;
        s.src = '//code.jivosite.com/script/widget/' + widget_id;
        var ss = document.getElementsByTagName('script')[0];
        ss.parentNode.insertBefore(s, ss);
    }
    if (d.readyState === 'complete') {
        l();
    } else {
        if (w.attachEvent) {
            w.attachEvent('onload', l);
        } else {
            w.addEventListener('load', l, false);
        }
    }
})();

// For the _UploadFileButton.cshtml
// This code is here, because this code doesn't work inside the Scripts section inside the _UploadFileButton.cshtml partial view.
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

