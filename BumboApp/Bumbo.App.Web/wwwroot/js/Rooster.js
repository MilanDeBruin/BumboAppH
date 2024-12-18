document.addEventListener("DOMContentLoaded", (event) => {

    setModalJS();
});


function setModalJS() {
    modal = document.getElementById("scheduleModal")
    modalCloselBtn = document.getElementById("closeScheduleModal")
    modalCancelBtn = document.getElementById("cancelScheduleModal")

    modalCancelBtn.addEventListener('click', () => {
        modal.style.display = "none";
    })
    modalCloselBtn.addEventListener('click', () => {
        modal.style.display = "none";
    })

    addBtn = document.getElementById("openModalBtn");
    addBtn.addEventListener('click', () => {
        modal.style.display = "block";
    })
}
