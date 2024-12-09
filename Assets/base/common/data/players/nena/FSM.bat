SET PRJDIR=%CODERCHILDDEV%\projects\MariaAkane\trunk\

%PRJDIR%\toolchain\qfsm2animcontroller.exe AnimController-platformer_ladder.fsm
copy /y AnimController-platformer_ladder.inc %PRJDIR%\src\MainProject\src\Game\Entities\Component\Misc
%PRJDIR%\toolchain\qfsm2animcontroller.exe AnimController-platformer_side.fsm
copy /y AnimController-platformer_side.inc %PRJDIR%\src\MainProject\src\Game\Entities\Component\Misc
%PRJDIR%\toolchain\qfsm2animcontroller.exe AnimController-hack_slash.fsm
copy /y AnimController-hack_slash.inc %PRJDIR%\src\MainProject\src\Game\Entities\Component\Misc
%PRJDIR%\toolchain\qfsm2animcontroller.exe AnimController-shooter.fsm
copy /y AnimController-shooter.inc %PRJDIR%\src\MainProject\src\Game\Entities\Component\Misc
%PRJDIR%\toolchain\qfsm2animcontroller.exe AnimController-shmup.fsm
copy /y AnimController-shmup.inc %PRJDIR%\src\MainProject\src\Game\Entities\Component\Misc
pause