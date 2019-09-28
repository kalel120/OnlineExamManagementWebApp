// IIFE
(($) => {
    $(() => {
        const viewOrgModal = $("#js-modal-viewOrg");


        const getTableRowAsObject = (tableRow) => {
            return {
                Id: tableRow.find("td:eq(7)").children(".js-viewOrgModalPopup").attr("data-org-id"),
                Name: tableRow.find("td:eq(1)").text(),
                Code: tableRow.find("td:eq(2)").text(),
                Address: tableRow.find("td:eq(3)").text(),
                Contact: tableRow.find("td:eq(4)").text()
            };
        }

        const bindToViewOrgModal = (data) => {
            $("#js-modal-viewOrg-name").val(data.Name);
            $("#js-modal-viewOrg-code").val(data.Code);
            $("#js-modal-viewOrg-address").val(data.Address);
            $("#js-modal-viewOrg-contact").val(data.Contact);
        }

        $(document).on("click", ".js-viewOrgModalPopup", (event) => {
            setTimeout(() => {
                bindToViewOrgModal(getTableRowAsObject($(event.target).closest("tr")));
                $(".js-modal-viewOrg-logo").attr("src", $(event.target).closest("tr").find("td:eq(6) img").attr("src"));
                viewOrgModal.modal("toggle");
            }, 300);
        });
    });
})(jQuery);