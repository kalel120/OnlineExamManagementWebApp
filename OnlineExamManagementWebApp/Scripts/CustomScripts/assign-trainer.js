
$(function () {
    var organizationId = $('#OrganizationId').val();
    var courseId = $('#Id').val();

    // This empty array will contain list of new trainers to be added in html table
    var trainers = new Array();

    function clearCheckBox() {
        $("#is-lead").prop('checked', false);
    }

    function clearTrainersDdl() {
        $("#js-ddl-trainers").empty();
    }


    $('#js-ddl-trainers').select2({
        placeholder: "--Select Trainer--",
        ajax: {
            url: "/Course/GetTrainersByOrganization",
            dataType: "json",
            data: function (params) {
                var query = {
                    searchTerm: params.term,
                    orgId: organizationId
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: data
                };
            }
        }
    });

    // Load assigned trainer from database in assigned trainer tab
    loadTrainersById(courseId);

    function loadTrainersById(courseId) {
        clearCheckBox();
        clearTrainersDdl();

        $.ajax({
            url: "/Course/GetTrainersByCourse/" + courseId,
            type: "GET",
            contentType: 'application/json',
            success: function (result) {
                let html = "";

                $.each(result, function (key, item) {
                    var rbValue = '<td><input type="radio" name="IsLead" disabled="disabled"/> </td>';
                    if (item.IsLead) {
                        rbValue = '<td><input type="radio" name="IsLead" disabled="disabled" checked="checked"/> </td>';
                    }

                    html += `"<tr>
                     <td><input type=hidden value='${item.TrainerId}'/> ${item.TrainerName} </td>
                     ${rbValue}
                     <td> 
                        <a href='#' class='js-toggle-lead'> Edit |</a>
                        <a href='#' class='js-remove-trainer'> Remove </a></td>";
                    </tr>"`;
                });

                $('#table-body').html(html);
            },

            error: function () {
                alert('Something is wrong!');
            }
        });
    }

    function isTrainerAlreadyExists(selectedTrainerId) {
        var isDuplicate = false;
        $("#table-body").find('tr').each(function () {
            var td = $(this).find('input[type="hidden"]').val();
            if (td === selectedTrainerId) {
                isDuplicate = true;
            }
            if (isDuplicate) {
                return;
            }
        });
        return isDuplicate;
    }

    function isLeadTrainerExists() {
        if ($("input[name='IsLead']:checked").val()) {
            return true;
        }
        return false;
    }

    $('#js-insert-table').on('click', function () {
        var selectedTrainerId = $('#js-ddl-trainers').val();

        if (selectedTrainerId === null || selectedTrainerId === "0") {
            alert("Select a trainer to assign");
            clearTrainersDdl();
            return false;
        }

        var selectedTrainerName = $('#js-ddl-trainers').children("option").filter(":selected").text();
        var isLeadCheckBoxChecked = $('#is-lead').prop('checked');
        
        if (isTrainerAlreadyExists(selectedTrainerId)) {
            alert("Already exists");
            clearCheckBox();
            clearTrainersDdl();
            return false;
        }

        if (isLeadTrainerExists() && isLeadCheckBoxChecked) {
            alert('Lead already defined');
            clearCheckBox();
            return false;
        }

        // Building htmlTable;
        let rbValue = '<td><input type="radio" name="IsLead" disabled="disabled"/> </td>';
        if (isLeadCheckBoxChecked) {
            rbValue = '<td><input type="radio" name="IsLead" disabled="disabled" checked="checked"/> </td>';
        }

        let html = `"<tr>
                       <td><input type='hidden' value='${selectedTrainerId}'/> ${selectedTrainerName} </td>
                       ${rbValue}
                       <td>
                           <a href='#' class='js-toggle-lead'> Edit |</a>
                           <a href='#' class='js-remove-trainer'> Remove </a>
                       </td>
                      </tr>"`;

        $("#table-body").append(html);

        var assignVm = {
            CourseId: courseId,
            TrainerName: selectedTrainerName,
            TrainerId: selectedTrainerId,
            IsLead: isLeadCheckBoxChecked
        }
        trainers.push(assignVm);

        clearCheckBox();        
    });


    // Save to database functionality
    $("#js-save-trainers").click(function () {
        if (!trainers || trainers.length === 0) {
            alert("Something went wrong.\nAdd Proper trainer from list");
            return;
        }

        if (!confirm("Are you sure you want to assign?")) {
            return;
        }

        $.ajax({
            url: "/Course/AssignTrainer",
            type: "POST",
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(trainers),
            success: function () {
                trainers.length = 0;
                loadTrainersById(courseId);
                alert("Assigned Trainers Successfully");
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
    });

    // Remove trainer from html table
    $(document).on('click', '.js-remove-trainer', function () {
        var closestRow = $(this).closest("tr");
        var trainerId = closestRow.find("input[type='hidden']").val();
        var trainerName = closestRow.find('td:eq(0)').text();

        var removableTrainer = {
            CourseId: courseId,
            TrainerId: trainerId
        };

        if (confirm("Are you sure you want to delete?")) {                        
            removeFromUnsavedList(removableTrainer);
            closestRow.remove();            
            removeTrainerFromDb(removableTrainer, trainerName);
        }
    });


    function removeTrainerFromDb(removable,trainerName) {
        $.ajax({
            url: '/Course/UnassignTrainer',
            type: "POST",
            contentType: 'application/json',
            dataType: "json",
            data: JSON.stringify(removable),
            success: function (result) {
                alert(trainerName+" is removed");
            },
            error: function (message) {
                console.log(message.responseText);
            }
        });

    }

    function removeFromUnsavedList(removable) {
        trainers = trainers.filter(t =>  t.TrainerId !== removable.TrainerId);
    }


    // JS related to edit modal  popup//
    var editTrainerModal = $("#js-toggle-lead-modal");
    var updateLeadStatusButton = $("#js-modal-editTrainer-updateStatus");

    $(document).on('click', '.js-toggle-lead', function (event) {
        editTrainerModal.modal("toggle");
        updateLeadStatusButton.prop("disabled", true);
        loadTrainerLeadStatusToggleModal(event);
    });

    function loadTrainerLeadStatusToggleModal(e) {
        var editLink = $(e.target).closest("tr");

        $("#js-editing-trainerId").val(editLink.find("input[type='hidden']").val());
        $("#js-p-trainerName").text(editLink.find("td:eq(0)").text());
        $("#js-modal-editTrainer-leadStatus").prop("checked", editLink.find("input[name = 'IsLead']").prop("checked"));
    }

    $("#js-modal-editTrainer-leadStatus").on('change', function () {
        updateLeadStatusButton.prop("disabled", false);
    });

    updateLeadStatusButton.on('click', function () {
        var isLeadStatusChecked = $("#js-modal-editTrainer-leadStatus").prop("checked");
        var editingTrainerId = $("#js-editing-trainerId").val();

        if (isLeadTrainerExists() && isLeadStatusChecked) {
            alert("More than one lead is not permissible");
            editTrainerModal.modal("hide");
            return;
        }

        var updatableCourseTrainer = {
            CourseId: courseId,
            TrainerId: editingTrainerId,
            IsLead: isLeadStatusChecked
        };

        updateLeadTrainerStatus(updatableCourseTrainer);
    });

    function updateLeadTrainerStatus(jsonObject) {
        $.ajax({
            url: '/Course/UpdateLeadTrainer',
            type: 'POST',
            contentType: 'application/json',
            dataType: "json",
            data: JSON.stringify(jsonObject),

            success: function (result) {
                if (!result) {
                    alert("Save failed. Trainer is not assigned yet!");
                    editTrainerModal.modal("hide");
                    return;
                }
                alert("Updated");
                editTrainerModal.modal("hide");
                loadTrainersById(courseId);
            },
            error: function (message) {
                console.log(message.responseText);
            }
        });
    }
    // JS related to edit modal  popup//

    // jQuery function ends here //
});