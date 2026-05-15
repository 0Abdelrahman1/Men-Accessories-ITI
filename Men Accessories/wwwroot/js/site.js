// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleFavorite(productId, btnElement) {
    var btn = $(btnElement);
    var svg = btn.find('svg');

    var currentFill = svg.attr('fill');
    var currentStroke = svg.attr('stroke');


    var isCurrentlyFav = currentFill === '#e05252' || currentFill === 'var(--error)';
    if (isCurrentlyFav) {
        svg.attr('fill', 'none');
        svg.attr('stroke', 'currentColor');
    } else {
        svg.attr('fill', '#e05252');
        svg.attr('stroke', '#e05252');
    }

    btn.prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/Favorites/Toggle",
        data: { productId: productId },
        success: function (response) {
            if (!response.success) {
                svg.attr('fill', currentFill);
                svg.attr('stroke', currentStroke);

                if (response.message === "Please login first") {
                    window.location.href = "/Account/Login";
                } else {
                    alert(response.message);
                }
            }
        },
        error: function (xhr) {
            svg.attr('fill', currentFill);
            svg.attr('stroke', currentStroke);

            if (xhr.status === 401 || xhr.status === 302) {
                window.location.href = "/Account/Login";
            } else {
                console.error("AJAX Error");
            }
        },
        complete: function () {
            btn.prop('disabled', false);
        }
    });
}