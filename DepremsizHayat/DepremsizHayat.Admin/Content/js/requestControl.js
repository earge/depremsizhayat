(function () {
    Array.from(document.querySelectorAll("[data-edit]")).forEach(item => {
        item.addEventListener("click", function () {
            editAndSave(item)
        })
    })
})()

function editAndSave(item) {
    let form = item.parentElement
    if (item.dataset.edit == "0") {
        item.innerHTML = '<i class="fas fa-check"></i>'
        item.dataset.edit = "1"

    }
    else if (item.dataset.edit == "1") {
        form.submit()


        item.innerHTML = '<i class="fas fa-pen"></i>'
        item.dataset.edit = "0"
    }
}



$.ajax({
    url: "GetRequests",
    type: "POST",
    contentType: false,
    dataType: "json",
    success: function (data) {
        let table = document.createElement("table")
        table.classList.add("table", "table-bordered")
        data.forEach(items => {
            let row = 
`
<tr>
<td><ro-checkbox group="request" value="${items.ANALYSIS_REQUEST_ID}" data-status="${items.STATUS_ID}"></td>
<td>${items.FIRST_NAME}</td>
<td>${items.LAST_NAME}</td>
<td>${items.COUNTRY}</td>
<td>${items.DISTRICT}</td>
<td>${ConvertDate(items.CREATED_DATE)}</td>
<td><textarea disabled>${items.ADDRESS}</textarea></td>
<td>${items.NUMBER_OF_FLOORS}</td>
<td>${items.YEAR_OF_CONSTRUCTION}</td>
<td><input type="text" value="${items.PHONE_NUMBER_1}" disabled /></td>
<td><input type="text" value="${items.PHONE_NUMBER_2}" disabled /></td>
<td><input type="text" value="${items.USER_NOTE}" disabled /></td>
<td>${items.STATUS_NAME}</td>
</tr>
`
            document.querySelector("#requestsTable").innerHTML += row
        })
        
    }
})