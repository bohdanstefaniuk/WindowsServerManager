Instruction for use unit test in this project.

First you need to run PrepareTestData project.
If you use CI for runing unit test, you must create build step with this project and run before unit projects starts.
This project adds some test data for successful tests.
List of test data:
1. Application pool "WindowsServerManager_UnitTest_Pool"
2. Application "WindowsServerManager_UnitTest_Application"
3. Site "WindowsServerManager_UnitTest_Site"
4. Test settings
6. ...


UnitTestUtil
Special utils for better experience