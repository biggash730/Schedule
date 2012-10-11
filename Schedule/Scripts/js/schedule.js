
$('#schDateOnce').datePicker();
function ShowAddScheduleModal() {
    var addSchForm = document.forms['addScheduleForm'];
    $('#addSchedule').modal('show');

    $('#addSchedule').find('#schContinue').click(function () {

        var schType = addSchForm.elements["schType"].value;
        var schName = addSchForm.elements["schName"].value;
        var schDescription = $('#schDescription').val();

        $('#addSchedule').modal('hide');
        
        if (!schName) {
            alert("Please enter the schedule name");
            $('#addSchedule').modal('show');
        }
        else if (!schDescription) {
            alert("Please enter the description");
            $('#addSchedule').modal('show');
        } else if (schType == "ONCE") {
            ShowAddScheduleTimeOnceModal(schName, schDescription);
        }
        else if (schType == "DAILY") {
            ShowAddScheduleTimeDailyModal(schName, schDescription);
        }
        else if (schType == "WEEKLY") {
            ShowAddScheduleTimeWeeklyModal(schName, schDescription);
        }
    });
}

function ShowAddScheduleTimeOnceModal(schName, schDescription) {

    $('#addScheduleTimeOnce').modal('show');
    
    var addScheduleTimeOnceForm = document.forms['addScheduleTimeOnceForm'];

    addScheduleTimeOnceForm.elements["schNameOnce"].value = schName;
    //addScheduleTimeOnceForm.elements["schDescriptionOnce"].value = schDescription;
    $('#schDescriptionOnce').val(schDescription);
}

function ShowAddScheduleTimeDailyModal(schName, schDescription) {
    
    $('#addScheduleTimeDaily').modal('show');   
    var addScheduleTimeDailyForm = document.forms['addScheduleTimeDailyForm'];
    
    addScheduleTimeDailyForm.elements["schNameDaily"].value = schName;
    //addScheduleTimeDailyForm.elements["schDiscriptionDaily"].value = schDescription;
    $('#schDescriptionDaily').val(schDescription);
}

function ShowAddScheduleTimeWeeklyModal(schName, schDescription) {

    $('#addScheduleTimeWeekly').modal('show');
    var addScheduleTimeWeeklyForm = document.forms['addScheduleTimeWeeklyForm'];
    
    addScheduleTimeWeeklyForm.elements["schNameWeekly"].value = schName;
    //addScheduleTimeWeeklyForm.elements["schDiscriptionWeekly"].value = schDescription;
    $('#schDescriptionWeekly').val(schDescription);
}

//....................................................................................................

function ShowScheduleDetailsModal(id) {

    $.ajax({
        url: "/Home/ViewSchedule/" + id,
        type: "POST",
        dataType: "json",
        success: function (result) {
            alert(result.schType);
//            if (result.schType == "Once") {
//                ShowScheduleTimeOnceDetailsModal(id, result.schName, result.schDescription);
//            }

        }
    });

}

function ShowScheduleTimeOnceDetailsModal(id, schName, schDescription) {
    $('#ViewScheduleTimeOnce').modal('show');
}

function ShowScheduleTimeDailyDetailsModal(id) {
    $('#ViewScheduleTimeDaily').modal('show');
}

function ShowScheduleTimeWeeklyDetailsModal(id) {
    $('#ViewScheduleTimeWeekly').modal('show');
}