function showToast(message, isSuccess = true, delay = 2000) {

    document.querySelectorAll('.toast').forEach(t => t.closest('.position-fixed')?.remove());
    const toastHTML = `
        <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 1100">
            <div class="toast align-items-center border-1 purple-background 
                ${isSuccess ? 'text-black' : 'text-danger fw-bold'} border-0 show" 
                role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body bg-white">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>`;

    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = toastHTML;
    const toastContainer = tempDiv.firstElementChild;
    document.body.appendChild(toastContainer);

    const toastEl = toastContainer.querySelector('.toast');
    const toast = new bootstrap.Toast(toastEl, { delay: delay });
    toast.show();

    toastEl.addEventListener('hidden.bs.toast', () => {
        toastContainer.remove();
    });
}