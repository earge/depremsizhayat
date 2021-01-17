let digitOne = document.querySelector("#digitOne")
    let digitTwo = document.querySelector("#digitTwo")
    let digitTree = document.querySelector("#digitTree")
    let digitFour = document.querySelector("#digitFour")
    let digitFive = document.querySelector("#digitFive")
    let digitSix = document.querySelector("#digitSix")

    function setInputFilter(textbox, inputFilter) {
      ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function(event) {
        textbox.addEventListener(event, function() {
          if (inputFilter(this.value)) {
            this.oldValue = this.value;
            this.oldSelectionStart = this.selectionStart;
            this.oldSelectionEnd = this.selectionEnd;
          } else if (this.hasOwnProperty("oldValue")) {
            this.value = this.oldValue;
            this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
          } else {
            this.value = "";
          }
        });
      });
    }
    setInputFilter(digitOne, function(value) {return /^\d*$/.test(value); });
    setInputFilter(digitTwo, function(value) {return /^\d*$/.test(value); });
    setInputFilter(digitTree, function(value) {return /^\d*$/.test(value); });
    setInputFilter(digitFour, function(value) {return /^\d*$/.test(value); });
    setInputFilter(digitFive, function(value) {return /^\d*$/.test(value); });
    setInputFilter(digitSix, function(value) {return /^\d*$/.test(value); });

    function moveNext(baseElement,nextElement) {
      baseElement.addEventListener("keyup",function(event){
        if(this.value.length == 1) { nextElement.focus() }
      })
    }

    function moveBack(baseElement,backElement){
      baseElement.addEventListener("keyup",function(event){
        if(event.keyCode == 8 && this.value.length == 0) { backElement.focus() }
      })
    }
  
    moveNext(digitOne,digitTwo)
    moveNext(digitTwo,digitTree)
    moveNext(digitTree,digitFour)
    moveNext(digitFour,digitFive)
    moveNext(digitFive,digitSix)

    moveBack(digitSix,digitFive)
    moveBack(digitFive,digitFour)
    moveBack(digitFour,digitTree)
    moveBack(digitTree,digitTwo)
    moveBack(digitTwo,digitOne)


    function handlePaste (e) {
        var clipboardData, pastedData;

        e.stopPropagation();
        e.preventDefault();

        clipboardData = e.clipboardData || window.clipboardData;
        pastedData = clipboardData.getData('Text');

        let code = Number(pastedData)
        if(code && code.toString().length==6){
          let codeString = code.toString().split('')
          digitOne.value = codeString[0]
          digitTwo.value = codeString[1]
          digitTree.value = codeString[2]
          digitFour.value = codeString[3]
          digitFive.value = codeString[4]
          digitSix.value = codeString[5]
          digitSix.focus()
        }
    }

    digitOne.addEventListener('paste', handlePaste);