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
<td>${items.FIRST_NAME} ${items.LAST_NAME}</td>
<td>
<div>
    <b>Ülke : </b>
    ${items.COUNTRY}<br>
    <b>Şehir : </b>
    ${items.DISTRICT}<br>
</div>
</td>
<td>
<div>
    <b>Kat Sayısı : </b>
    ${items.NUMBER_OF_FLOORS}<br>
    <b>Yapım Yılı : </b>
    ${items.YEAR_OF_CONSTRUCTION}<br>
</div>
</td>
<td>${items.STATUS_NAME}</td>
<td><button data-analyseid="${items.ANALYSIS_REQUEST_ID}" class="detailButton">Detay</button></td>
</tr>
`
            document.querySelector("#requestsTable").innerHTML += row
        })
        Array.from(document.querySelectorAll(".detailButton")).forEach(item => {
    item.addEventListener("click", function () { editRequest(item.dataset.analyseid) })
})
    }
})




function editRequest(id) {
    let request = document.createElement("template")
    $.ajax({
        url: "RequestDetail?id="+id,
        type: "POST",
        contentType: false,
        success: function (data) {
            request.innerHTML =
                `
                <button data-case="0" id="reques-detail-edit">Düzenle</button>
                Telefon 1: <input type="text" value=${data.PHONE_NUMBER_1} class="request-detail-input" disabled>
                Telefon 2: <input type="text" value=${data.PHONE_NUMBER_2} class="request-detail-input" disabled>
                Adres: <textarea class="request-detail-input" disabled>${data.ADDRESS}</textarea>
                Açıklama : <textarea class="request-detail-input" disabled>${data.USER_NOTE}</textarea>
                ${ConvertDate(data.CREATED_DATE)}
                `
            Prompt.show(request)
            addEventToButton()
        }
    })
}

function addEventToButton() {
    let requestEditButton = document.querySelector("#reques-detail-edit")
    requestEditButton.addEventListener("click", function (event) {
        if (event.target.dataset.case == "0") {
            Array.from(document.querySelectorAll(".request-detail-input")).forEach(item => item.disabled = false)
            event.target.innerHTML = "Güncelle"
            event.target.dataset.case == "1"
        } else if (event.target.dataset.case == "1") {
            Array.from(document.querySelectorAll(".request-detail-input")).forEach(item => item.disabled = true)
            event.target.innerHTML = "Düzenle"
            event.target.dataset.case == "0"
        }
    })
}
