// IIFE to wrap jQuery code

(($) => {
    $(() => {
        // For every call of this function, questions serial no. will reset using loop index
        const reOrderQuestionSerialOnClient = function () {
            $("#tbl-questions").find("tbody tr td:nth-child(1)").each(function (index, element) {
                console.log(typeof index);
                $(element).text(index + 1);
            });
        };

        $(document).on("click", ".js-removeQuestion", function (event) {
            let eventTarget = $(event.target);

            let closestRow = eventTarget.closest("tr");

            bootbox.confirm({
                size: "large",
                message: "Are you sure? This will delete this particular question",
                callback: async function (confirmed) {
                    if (!confirmed) {
                        return;
                    }

                    // remove from questions table (client)
                    closestRow.remove();

                    // re-order serial no. on client
                    reOrderQuestionSerialOnClient();

                    // marked as removed on server
                    // re-sequence on server
                    // refresh questions table (from serve)
                    if (typeof localStorage !== "undefined") {
                        localStorage.setItem("success", "Question Removed");
                    }
                    window.location.reload();
                }
            });
        });
    });
})(jQuery);