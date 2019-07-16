//Must be recompiled manually after every change due to the bug in the WebOptimizer tool,
//when js or jsx files cannot be auto-recompiled, i.e. rebundled and reminified, on saving.
//This comment can be removed after resolving the 'https://github.com/madskristensen/WebCompiler/issues/402' issue

'use strict';

$('.fa-angle-right').click(function () {
    $(this).toggleClass('fa-angle-right');
    $(this).toggleClass('fa-angle-down');
});

