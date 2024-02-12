using CodingTracker;

var database = new DatabaseManager();
database.CreateTable();

var userInput = new UserInput();
userInput.MainMenu();