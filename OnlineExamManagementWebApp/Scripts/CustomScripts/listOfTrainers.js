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
        /**DataTable END */

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

            bootbox.confirm({
                message: "Are you sure you want to delete this Trainer?",
                buttons: {
                    confirm: {
                        label: `<i class="fa fa-check"></i> Yes`,
                        className: "btn-danger"
                    }
                    , cancel: {
                        label: `<i class="fa fa-times"></i> No`,
                        className: "btn-success"
                    }
                },
                callback: (result) => {
                    if (result) {
                        deleteTrainerById(trainerId);
                    }
                }
            });
        });
        /** Delete Trainer END**/
    });
})(jQuery);