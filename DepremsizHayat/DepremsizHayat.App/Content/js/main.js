document.querySelector(".navbar .toggle").addEventListener("click",()=>{
  let navMenu = document.querySelector(".navbar .menu")
  if(!navMenu.classList.contains("block")){
    navMenu.classList.add("block")
  }
  else{
    navMenu.classList.remove("block")
  }
})

