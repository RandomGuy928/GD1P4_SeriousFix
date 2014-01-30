#pragma strict

function Update () {

   if (Input.GetKeyDown (KeyCode.LeftArrow)){
      BroadcastMessage ("PrePlay");
      BroadcastMessage ("PlayAnimation", 0);
      Left();
      }
   
   if (Input.GetKeyDown (KeyCode.RightArrow)){
      BroadcastMessage ("PrePlay");
      BroadcastMessage ("PlayAnimation", 1);
      Right();
      }
      
   if (Input.GetKeyDown (KeyCode.UpArrow)){
      BroadcastMessage ("PrePlay");
      BroadcastMessage ("PlayAnimation", 2);
      Foreward();
      
   }
   
   if (Input.GetKeyDown (KeyCode.DownArrow)){
      BroadcastMessage ("PrePlay");
      BroadcastMessage ("PlayAnimation", 3);
      Backward();
      
   }
   
   if (Input.GetKeyDown (KeyCode.Space)){
      BroadcastMessage ("SetSpeed", 1);
   }
   
   if (Input.anyKey == false){
      BroadcastMessage ("PrePlay");
      BroadcastMessage ("PlayAnimation", -1);
      BroadcastMessage ("SetSpeed", 10);
      StopAllCoroutines();
   }
}

function Foreward(){
   while (true){
      transform.position += Vector3.forward * Time.deltaTime;
      yield WaitForSeconds (0);
   }

   yield;
}

function Backward(){
   while (true){
      transform.position -= Vector3.forward * Time.deltaTime;
      yield WaitForSeconds (0);
   }

   yield;
}

function Left(){
   while (true){
      transform.position -= Vector3.right * Time.deltaTime;
      yield WaitForSeconds (0);
   }

   yield;
}

function Right(){
   while (true){
      transform.position += Vector3.right * Time.deltaTime;
      yield WaitForSeconds (0);
   }

   yield;
}