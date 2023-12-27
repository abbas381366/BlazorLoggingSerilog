function OpenModal(idmodal) {
    var myModal = new bootstrap.Modal(document.getElementById(idmodal));
    myModal.show();
    openedModal = myModal;
}
var openedModal;
function CloseModal(idmodal) {
    var myModalEl = document.getElementById(idmodal);
    var modal = bootstrap.Modal.getInstance(myModalEl); // Returns a Bootstrap modal instance
    modal.hide();
}

function ShowSweet(title, mess) {
    Swal.fire({ title: title, text: mess, icon: 'error', confirmButtonText: 'OK' });
}