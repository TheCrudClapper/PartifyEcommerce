function storeOriginalValue(input) {
    input.dataset.originalValue = input.value;
}
function submitIfChanged(input) {
    if (input.value !== input.dataset.originalValue) {
        //write request in ajax
        var form = $(input).closest('form');
        var quantity = input.value;
        var token = form.find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: form.attr('action'),
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            data: {
                quantity: quantity,
            },
            success: function (response) {
                if (response.success) {
                    $('#cartItemsContainer').load(location.href + ' #cartItemsContainer');
                    $('#priceSummaryContainer').load(location.href + ' #priceSummaryContainer > *');
                    $('#navbarCart').load('/Cart/GetCartItemsCount #navbarCart');
                    showToast(response.message, true);
                } else {
                    showToast(response.message, false);
                }
            },
            error: function () {
                showToast('Error updating quantity. Please try again.', false);
            }
        });
    }
}

$(document).on('click', '.deleteButton', function (e) {
    e.preventDefault();
    var offerId = $(this).data('id');
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Cart/DeleteFromCart/' + offerId,
        type: 'POST',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.success) {
                if ($('#cartItemsContainer .card').length == 1) {
                    location.reload();
                } else {
                    $('#cartItemsContainer').load(location.href + ' #cartItemsContainer');
                    $('#priceSummaryContainer').load(location.href + ' #priceSummaryContainer > *');
                    $('#navbarCart').load('/Cart/GetCartItemsCount #navbarCart');
                }
                showToast(response.message, true);
            } else {
                showToast(response.message, false);
            }
        },
        error: function (xhr) {
            showToast('Unexpected error occurred. Please try again.', false);
        }
    });
});