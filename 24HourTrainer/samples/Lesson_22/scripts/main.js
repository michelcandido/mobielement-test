function getCurrentTime() {
var theAM_PM;
var theDate = new Date();
var theHour = theDate.getHours();
if (theHour < 12) {
   theAM_PM = "AM";
   }
else {
   theAM_PM = "PM";
   }
if (theHour == 0) {
   theHour = 12;
   }
if (theHour > 12) {
   theHour = theHour - 12;
   }
var theMinutes = theDate.getMinutes();
theMinutes = theMinutes + "";
if (theMinutes < 10)
   {
   theMinutes = "0" + theMinutes;
   }
var theFullTime = theHour + ":" + theMinutes + " " + theAM_PM;
document.theForm.currentTime.value = theFullTime;
}
