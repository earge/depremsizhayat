function pagination(elementId, location) {
    var urlParams = new URLSearchParams(window.location.search);
    var p = urlParams.get('p')

    let element = document.querySelector(`#${elementId}`)
    var contentPerPage = element.dataset.contentPerpage
    var contentCount = element.dataset.contentCount

    if (!Number.isInteger(parseInt(p)) || parseInt(p) < 0) p = 1
    if (parseInt(p) > Math.ceil(contentCount / contentPerPage)) p = Math.ceil(contentCount / contentPerPage)

    let paginationDiv = document.createElement("div")
    paginationDiv.classList.add("pagination")


    let first = document.createElement("button")
    first.innerHTML = '<i class="fas fa-chevron-left"></i><i class="fas fa-chevron-left"></i>'

    let previous = document.createElement("button")
    previous.innerHTML = '<i class="fas fa-chevron-left"></i>'

    let pageSpan = document.createElement("span")
    pageSpan.innerHTML = `${p}/${Math.ceil(contentCount / contentPerPage)}`

    let next = document.createElement("button")
    next.innerHTML = '<i class="fas fa-chevron-right"></i>'

    let last = document.createElement("button")
    last.innerHTML = '<i class="fas fa-chevron-right"></i><i class="fas fa-chevron-right"></i>'

    if (p == 1) { previous.disabled = true; first.disabled = true }
    if (p == Math.ceil(contentCount / contentPerPage)) { next.disabled = true; last.disabled = true }

    first.onclick = function () {
        window.location = location + "?p=1"
    }
    previous.onclick = function () {
        window.location = location + "?p=" + (p - 1)
    }
    next.onclick = function () {
        window.location = location + `?p=${parseInt(p) + 1}`
    }
    last.onclick = function () {
        window.location = location + `?p=${Math.ceil(contentCount / contentPerPage)}`
    }

    paginationDiv.appendChild(first)
    paginationDiv.appendChild(previous)
    paginationDiv.appendChild(pageSpan)
    paginationDiv.appendChild(next)
    paginationDiv.appendChild(last)
    element.appendChild(paginationDiv)
}
