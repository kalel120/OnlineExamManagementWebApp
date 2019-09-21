// IIFE
(($) => {
    $(() => {
        const orgId = $("#js-org-id").val();

        /**DataTable */
        let listOfTrainersDataTable = $("#tbl-listOfTrainers").DataTable({
            processing: true,
            ajax: {
                url: "http://localhost:52119/api/trainer/" + orgId,
                dataSrc: ""
            },
            columns: [
                { "data": "Id" },
                { "data": "Name" },
                { "data": "Contact" },
                { "data": "Email" },
                { "data": "Address" },
                { "data": "AlternateAddress" },
                { "data": "City" },
                { "data": "PostalCode" },
                { "data": "Country" },
                { "data": "OrganizationId" },
                {
                    "data": null,
                    "render": function (data, type, row) {
                        return `<input type="hidden" value="${row.Id}"/><input type="button" class="btn btn-warning js-editTrainerModalPopup" value="Edit" /> | 
                                <input type="button" class="btn btn-danger js-deleteTrainerPopup" value="Delete" />`;
                    }
                }
            ]
            , columnDefs: [{
                "searchable": false,
                "orderable": false,
                "targets": 0
            }],
            order: [[1, "asc"]],
            language: {
                "loadingRecords": "&nbsp;",
                "processing": '<div class="spinner"></div>'
            }
        });

        listOfTrainersDataTable.on("order.dt search.dt", function () {
            listOfTrainersDataTable.column(0, { search: "applied", order: "applied" })
                .nodes()
                .each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
        }).draw();
        /**END */

        const deleteTrainerDialog = $("#js-deleteTrainer-dialog");
        deleteTrainerDialog.hide();


        /**
         * Delete Trainer Functionality
         */
        const deleteTrainerById = (id) => {
            let dto = new Object();
            dto.TrainerId = id;
            $.ajax({
                url: "/api/trainer/",
                type: "DELETE",
                dataType: "json",
                data: dto
            })
                .done(async (data) => {
                    await listOfTrainersDataTable.ajax.reload();
                    setTimeout(() => {
                        alert("Trainer Deleted");
                    }, 300);
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
                    duration: 150
                },
                hide: {
                    effect: "explode",
                    duration: 150
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
        /**END**/
    });
})(jQuery);