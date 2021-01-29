document.querySelector(".navbar .toggle").addEventListener("click",()=>{
  let navMenu = document.querySelector(".navbar .menu")
  if(!navMenu.classList.contains("block")){
    navMenu.classList.add("block")
  }
  else{
    navMenu.classList.remove("block")
  }
})

function infoBoxesCleaner() {
    Array.from(document.querySelectorAll(".info-boxes:not(.json)")).forEach(item => { item.remove() })
}

if (document.querySelector(".profile") !== null) {
    document.querySelector(".profile").addEventListener("click",open,false)
}

function open(event) {
    event.stopPropagation()
    document.querySelector(".profil-box-open").classList.add("block")
    document.addEventListener("click",close,false)
}

function close() {
    document.querySelector(".profil-box-open").classList.remove("block")
    document.removeEventListener("click", close, false)
}


