// IIFE
(($) => {
    $(() => {
        $("#tbl-listOfTrainers").DataTable();
        const deleteTrainerDialog = $("#js-deleteTrainer-dialog");
        deleteTrainerDialog.hide();

        // Delete trainer with jquery UI dialog
        const deleteTrainerById = (id) => {
            $.post("/.../Delete/", { orgId: id })
                .done((data) => {
                    if (data) {
                        alert("Deleted Successfully");
                        location.reload(true);
                    } else {
                        alert("Deletion failed");
                    }
                })
                .fail((jqXhr, textStatus) => {
                    alert(`Error >>${textStatus}`);
                });
        }

        $(document).on("click", ".js-deleteTrainerPopup", (event) => {
            const trainerId = $(event.target).closest("tr").find('input[type="hidden"]').val();

            deleteTrainerDialog.dialog({
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                show: {
                    effect: "puff",
                    duration: 250
                },
                hide: {
                    effect: "explode",
                    duration: 250
                },
                buttons: [
                    {
                        text: "Delete?",
                        open: function () {
                            $(this).addClass("cancelClass");
                        },
                        click: function () {
                            deleteTrainerById(trainerId);
                            $(this).dialog("close");
                        }
                    }, {
                        text: "Cancel",
                        open: function () {
                            $(this).addClass("confirmClass");
                        },
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
            });
        });
        // delete trainer functionality End
    });
})(jQuery);

