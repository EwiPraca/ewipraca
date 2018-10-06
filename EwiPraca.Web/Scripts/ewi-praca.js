function showLoader() {
    $(".loader-container").fadeIn(100);
}
function hideLoader() {
    $(".loader-container").fadeOut(100);
}

$.fn.addBack = function (selector) {
    return this.add(selector == null ? this.prevObject : this.prevObject.filter(selector));
}

function onFailureModalEdit(e) {
    BootstrapDialog.show({
        title: 'Informacja',
        message: e.Message
    });
}