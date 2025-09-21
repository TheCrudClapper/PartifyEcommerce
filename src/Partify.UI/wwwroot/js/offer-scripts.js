
$(document).on('click', '.addToCartButton', function (e) {
    e.preventDefault();
    var offerId = $(this).data('id');
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Cart/AddToCart/' + offerId,
        type: 'POST',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.success) {
                $('#navbarCart').load('/Cart/GetCartItemsCount #navbarCart');
                showToast(response.message, true);
            } else {
                showToast(response.message, false);
            }
        },
        error: function () {
            showToast("Unexpected error occurred.Try again later", false);
        }
    });

});


