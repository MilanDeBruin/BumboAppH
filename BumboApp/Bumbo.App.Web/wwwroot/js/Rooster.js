document.addEventListener("DOMContentLoaded", (event) => {

    setModalJS();
});


function setModalJS() {
    modal = document.getElementById("scheduleModal")
    modalCloselBtn = document.getElementById("closeScheduleModal")
    modalCancelBtn = document.getElementById("cancelScheduleModal")

    document.getElementById("Opslaan").addEventListener("click", () => saveSchedule());

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

function saveSchedule() {
    const dayMapping = { 'Maandag': 0, 'Dinsdag': 1, 'Woensdag': 2, 'Donderdag': 3, 'Vrijdag': 4, 'Zaterdag': 5, 'Zondag': 6 };

    const department = document.getElementById("modalDepartment").value;
    const employeeId = document.getElementById("modalEmployees").value;
    const day = document.getElementById("modalDay").value;
    const dayName = getDayNameFromDate(day);
    const dayNumber = dayMapping[dayName];
    const startTime = document.getElementById("modalStartTime").value;
    const endTime = document.getElementById("modalEndTime").value;

    const start = parseTime(startTime);
    const end = parseTime(endTime);


    if (start >= end) {
        alert("Eind tijd moet later zijn dan start tijd.");
        return;
    }

    const baseDate = document.getElementById("modalDay").value;
    const dateObj = new Date(baseDate);
    updatedDate = dateObj.toISOString().split('T')[0];

    const scheduleEntry = {
        EmployeeId: parseInt(employeeId),
        Date: new Date(updatedDate).toISOString(),
        StartTime: startTime + ":00",
        EndTime: endTime + ":00",
        Department: department
    };
    const addScheduleUrl = document.getElementById('addScheduleUrl').getAttribute('data-url');

    $.ajax({
        url: addScheduleUrl,
        type: 'POST',
        data: JSON.stringify(scheduleEntry),
        contentType: 'application/json',
        success: function (response) {
            console.log("Response from server:", response);

            if (response.success) {
                location.reload();  // Dit vernieuwt de pagina
            } else {

                console.error("Failed to save schedule:", response.message);
                alert(`Failed to save the schedule: ${response.message}`);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error saving schedule:", status, error);
            alert("An unexpected error occurred. Please try again.");
        }
    });
}

function parseTime(timeStr) {
    const timeParts = timeStr.split(":");
    const hours = parseInt(timeParts[0], 10);
    const minutes = parseInt(timeParts[1], 10);
    return new Date(0, 0, 0, hours, minutes);
}

function getDayNameFromDate(dateString) {
    const date = new Date(dateString);
    const days = ['Zondag', 'Maandag', 'Dinsdag', 'Woensdag', 'Donderdag', 'Vrijdag', 'Zaterdag'];

    return days[date.getDay()];
}
