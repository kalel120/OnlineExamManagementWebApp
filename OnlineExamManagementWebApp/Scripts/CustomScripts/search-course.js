// I used IIFE around the jQuery code to avoid conflict with other js libraries

($(function () {
    $('#tbl-courseSearchResult').DataTable();

    const jsTrainerList = $('#js-trainersList');

    $(document.body).on("change", "#OrganizationId",
        function () {
            const orgId = $(this).val();

            if (orgId != undefined && orgId !== "") {
                let json = { id: orgId };

                $.ajax({
                    type: "POST",
                    url: `/Course/GetTrainersByOrganization/${orgId}`,
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(json),
                    success: function (data) {
                        jsTrainerList.empty();

                        $.each(data,
                            function (key, value) {
                                jsTrainerList.append(`"<option value="${value.Id}"> ${value.Name} </option>"`);
                            });
                    },
                    error: function () {
                        alert("Something is wrong");
                    }

                });
            } else {
                jsTrainerList.empty();
            }
        });
}))();