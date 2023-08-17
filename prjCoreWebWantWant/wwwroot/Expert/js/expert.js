var timeone = document.getElementById("timeone");
var times = document.getElementById("times");
var placeaddress = document.getElementById("placeaddress");
var response = document.getElementById("response");
var detail = document.getElementById("detail");

function clickdeploy(id,id2){
  
 
  
    if (id.style.display == "none"&&id2.value!=true) {
      id.style.display = "block";
      id2.style.display = "none";
      } else {
        id.style.display = "none";
        
      }

};
function clickdeployone(id){
  console.log("12345");
 
  
  if (id.style.display == "none") {
    id.style.display = "block";
   
    } else {
      id.style.display = "none";
      
    }

};






function clickdropexpert(id,id2,id3,id4,id5){
  
    id.style.display = "block";
    id2.style.display = "none";
    id3.style.display = "none";
    id4.style.display = "none";
    id5.style.display = "none";
  

};

