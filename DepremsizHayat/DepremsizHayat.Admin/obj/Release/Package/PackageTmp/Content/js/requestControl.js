/* GET REQUEST */
FillRequestsTable()
function FillRequestsTable() {
    $.ajax({
        url: "GetRequests",
        type: "POST",
        contentType: false,
        dataType: "json",
        beforeSend: function () {
            $.blockUI({ message: '<img src="/Content/svg/loading.svg">' })
            document.querySelector("#requestsTable").innerHTML = ""
        },
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
                    <b>Ülke: </b>${items.COUNTRY}<br>
                    <b>Şehir: </b>${items.DISTRICT}<br>
                </div>
                </td>
                <td>
                <div>
                    <b>Kat Sayısı: </b>${items.NUMBER_OF_FLOORS}<br>
                    <b>Yapım Yılı: </b>${items.YEAR_OF_CONSTRUCTION}<br>
                </div>
                </td>
                <td>${items.STATUS_NAME}</td>
                <td><button class="btn btn-primary detailButton" data-analyseid="${items.ANALYSIS_REQUEST_ID}">Detay</button></td>
                </tr>
                `
                document.querySelector("#requestsTable").innerHTML += row
            })
            Array.from(document.querySelectorAll(".detailButton")).forEach(item => {
                item.addEventListener("click", function () { editRequest(item.dataset.analyseid) })
            })
            $.unblockUI();
        }
    })
}


/* EDIT REQUEST */
function editRequest(id) {
    let request = document.createElement("template")
    $.ajax({
        url: "RequestDetail?id=" + id,
        type: "POST",
        contentType: false,
        beforeSend: function () {
            $.blockUI({ message: '<img src="/Content/svg/loading.svg">' })
        },
        success: function (data) {
            request.innerHTML =
                `
                <style>
                    .mb15{margin-bottom:15px}
                    .mt5{margin-top:5px}
                    .resize-none{resize:none}
                </style>
                <div class="mb15"><b>Telefon 1:</b> <input type="text" class="form-control request-detail-input mt5" id="newPhone1" value="${data.PHONE_NUMBER_1}" disabled></div>
                <div class="mb15"><b>Telefon 2:</b> <input type="text" class="form-control request-detail-input mt5" id="newPhone2" value="${data.PHONE_NUMBER_2}" disabled></div>
                <div class="mb15"><b>Adres:</b> <textarea id="newAddress" class="form-control resize-none request-detail-input mt5" disabled>${data.ADDRESS}</textarea></div>
                <div class="mb15"><b>Açıklama:</b> <textarea id="newNote" class="form-control resize-none request-detail-input mt5" disabled>${data.USER_NOTE}</textarea></div>
                <div class="mb15"><b>Oluşturulma Tarihi: </b>${ConvertDate(data.CREATED_DATE)}</div>
                <input type="hidden" value="${data.ANALYSIS_REQUEST_ID}" id="analysisId">
                <div><button data-case="0" id="reques-detail-edit" class="btn btn-primary">Düzenle</button></div>
                `
            Prompt.show(request)
            addEventToButton()
            $.unblockUI();
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
            let analysisId = document.querySelector("#analysisId")
            var model = {
                ANALYSIS_REQUEST_ID: analysisId.value,
                PHONE_NUMBER_1: newPhone1.value,
                PHONE_NUMBER_2: newPhone2.value,
                ADDRESS: newAddress.value,
                USER_NOTE: newNote.value,
            }
            event.target.disabled = true
            $.ajax({
                url: "EditRequest",
                data: model,
                type: "POST",
                beforeSend: function () {
                    $.blockUI({ message: '<img src="/Content/svg/loading.svg">' })
                },
                success: function (data) {
                    event.target.disabled = false
                    Array.from(document.querySelectorAll(".request-detail-input")).forEach(item => item.disabled = true)
                    event.target.innerHTML = "Düzenle"
                    event.target.dataset.case = 0
                    Notification.push("info", ArrayToString(data.Message))
                    $.unblockUI();
                }
            })
        }
    })
}

/* ALLOW-DENY */
document.querySelector("#allow").addEventListener("click", function () { execute("AllowRequests") })
document.querySelector("#deny").addEventListener("click", function () { execute("DenyRequests") })
function execute(action) {
    if (checkBoxProperties.getAllValue("request").length>0) {
        var model = { idList: checkBoxProperties.getAllValue("request") }
        $.ajax({
            url: action,
            type: 'POST',
            data: model,
            dataType: 'json',
            beforeSend: function () {
                $.blockUI({ message: '<img src="/Content/svg/loading.svg">' })
                document.querySelector("#allow").disabled = true
                document.querySelector("#deny").disabled = true
            },
            success: function (data) {
                if (data.Status) {
                    Notification.push("success", ArrayToString(data.Message))
                }
                else {
                    Notification.push("error", ArrayToString(data.Message))
                }
                FillRequestsTable()
                document.querySelector("#allow").disabled = false
                document.querySelector("#deny").disabled = false
                $.unblockUI();
            },
            error: function (error) {

            }
        })
    }
    else {
        Notification.push("error","Seçim yapmadınız.")
    }
}