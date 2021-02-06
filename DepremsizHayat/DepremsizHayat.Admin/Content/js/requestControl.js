
/* GET REQUEST */
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

/* EDIT REQUEST */
function editRequest(id) {
    let request = document.createElement("template")
    $.ajax({
        url: "RequestDetail?id=" + id,
        type: "POST",
        contentType: false,
        success: function (data) {
            request.innerHTML =
                `
                <div><button data-case="0" id="reques-detail-edit">Düzenle</button></div>
                <div>Telefon 1: <input type="text" id="newPhone1" value=${data.PHONE_NUMBER_1} class="request-detail-input" disabled></div>
                <div>Telefon 2: <input type="text" id="newPhone2" value=${data.PHONE_NUMBER_2} class="request-detail-input" disabled></div>
                <div>Adres: <textarea id="newAddress" class="request-detail-input" disabled>${data.ADDRESS}</textarea></div>
                <div>Açıklama : <textarea id="newNote" class="request-detail-input" disabled>${data.USER_NOTE}</textarea></div>
                <div>${ConvertDate(data.CREATED_DATE)}</div>
                <input type="hidden" value="${data.ANALYSIS_REQUEST_ID}">
                `
            Prompt.show(request)
            addEventToButton()
        }
    })
}

/* UPDATE REQUEST */
function addEventToButton() {
    let requestEditButton = document.querySelector("#reques-detail-edit")
    requestEditButton.addEventListener("click", function (event) {
        if (event.target.dataset.case == "0") {
            Array.from(document.querySelectorAll(".request-detail-input")).forEach(item => item.disabled = false)
            event.target.innerHTML = "Güncelle"
            event.target.dataset.case = 1
        } else if (event.target.dataset.case == "1") {
            let newPhone1 = document.querySelector("#newPhone1")
            let newPhone2 = document.querySelector("#newPhone2")
            let newAddress = document.querySelector("#newAddress")
            let newNote = document.querySelector("#newNote")
            var model = {
                PHONE_NUMBER_1: newPhone1,
                PHONE_NUMBER_2: newPhone2,
                ADDRESS: newAddress,
                USER_NOTE:newNote,
            }
            event.target.disabled = true
            $.ajax({
                url: "EditRequest",
                data: model,
                type: "POST",
                contentType: false,
                dataType: "json",
                success: function (data) {
                    event.target.disabled = false
                    Array.from(document.querySelectorAll(".request-detail-input")).forEach(item => item.disabled = true)
                    event.target.innerHTML = "Düzenle"
                    event.target.dataset.case = 0
                }
            })

            
        }
    })
}
