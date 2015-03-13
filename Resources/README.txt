Elliot Hatch, u0790511
Samuel Davidson, u0835059
December, 2014

Libraries - PS7.dll - StringSocket - updated December 1, 2014

BoggleClientForm
	Originally had a 2-window GUI, but we decided that one window provided a much better workflow.
	We used a TabController to switch between screens so that they could be easily created in the
	Windows Form designer and programatically switch screens easily.

December 1, 2014
	Built initial BoggleClient and GUI with basic play functionality, MVC design.

December 5, 2014
	Extensive bug testing, added unit tests, rebuilt GUI with TabController.
	Updated Server and Client protocol because the STOP message printed words and counts in the wrong order.
December 8, 2014
	Began the database aspect of the boggle server.
	Setup basic MySQL framework in the MySQL workbench and built the Web Server thread of the server.
December 11, 2014
	Redesigned the database to store minimal repeated information.
	Created the MySQL queries for the updated database for each of the 3 web requests.
	Finished a basic form of the HTML constructor for each website.
December 12, 2014
	Several bug fixes with the HTML and MySQL DLL.
	Added extensive CSS and HTML formatting to make the webpages look like the client.
	Added hyperlinks to all names and game ids for easy navigation.

Database Description:

	The database consists of four tables.

	Games:
		Stores the info unique to the game.
		Refered by the Players_Games table to get the full info for a game.
	Players:
		Stores the names of all the players.
		Basically gives a unique Id to each name so that it can be referenced in other databases without repeatedly storing the same VARCHAR.
	Players_Games:
		Connects the Players table with the Games table. 
		Each row contains a player, and a game they played in as well as the score they recieved and their unique wordId for their words in that game.
		Servers to link the other three tables together.
	Words:
		Stores a list of words for each game.
		The id is associated to a single player from a single game.
		Multiple words can have the same id which all point to the same player from the same game.
		Stores whether or not the word is valid.
	
	For webpage requests the webserver uses three different queries.
	The first one is for when a list of all boggle players is requested:

		SELECT name, 
		COUNT(CASE WHEN T1.score>(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesWon,
		COUNT(CASE WHEN T1.score<(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesLost,
		COUNT(CASE WHEN T1.score=(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesTied
		FROM Players INNER JOIN Players_Games T1 WHERE Players.id=T1.playerId GROUP BY Players.id ORDER BY gamesWon DESC

		This query has four different SELECT arguments.
		The first argument is clearly the name from each row.
		The second, third, and fourth argument use a sub-query to COUNT the number of times the current player has a greater than, less than or equal score of his opponent.
		The query makes use of variably named tables, inner joins, and orders based on the number of games won.

	The second query is for all games played by the specified player:
		
		SELECT gameId,
        timeCompleted,
        (SELECT name FROM Players INNER JOIN Players_Games T1 ON Players.id=T1.playerId WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) AS 'Opponent',
        score,
        (SELECT score FROM Players_Games T1 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) AS 'opponentScore'
        FROM Players INNER JOIN Players_Games T2 ON Players.id=T2.playerId INNER JOIN Games ON Games.id=T2.gameId WHERE name='@playername'

		This query has a SELECT with 5 different arguments.
		The first which is the game ID, the second which is the DateTime in which the game completed, and fourth which is the selected players score are all straightforward.
		The second and fourth arguments which are the opponent's name and score make use of a subquery in which the opponent's information must be found through following the game ID for each game.
		The @playername is replaced in the code with the name of the player that was specified in the request.

	The third query is for all the information of a specific game ID:
		
		SELECT timeCompleted, board FROM Games WHERE id=@gameID

		SELECT T2.playerId AS 'id',
        (SELECT name FROM Players INNER JOIN Players_Games T1 ON Players.id=T1.playerId WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) AS 'name',
        T2.score AS 'score'
        FROM Games INNER JOIN Players_Games T2 ON Games.id=T2.gameId INNER JOIN Players ON Players.id=T2.playerId WHERE Games.id=@gameID

		This query has two SELECTS in different lines of code.
		@gameID represents the gameId which is entered in the page request.
		The first is very simple and gets the time completed and board for the game ID.
		The second SELECT is a bit more complicated and utilizes 3 arguments.
		The first and third arguments and straightforward and grab the player ID and score.
		The second argument makes use of a subquery to return the name of the players.
		Unlike the previous two page request queries, this query returns two rows instead of one.
		There is a row for each player within the game of boggle.

	When a game is completed, a single query is used to INSERT all the information:

		 INSERT INTO Players (name) VALUES ('@playerOne') ON DUPLICATE KEY UPDATE name = '@playerOne';
		 INSERT INTO Players (name) VALUES ('@playerTwo') ON DUPLICATE KEY UPDATE name = '@playerTwo';
		 INSERT INTO Games (timeLimit,board) VALUES (10,'THEABCDEFGHIJKLM');
		 SET @gameID:=LAST_INSERT_ID();
		 INSERT INTO Players_Games (playerId, gameId, score) VALUES ((Select Players.id FROM Players WHERE name='@playerTwo'),@gameID, 0);
		 SET @firstWords:=LAST_INSERT_ID();
		 INSERT INTO Players_Games (playerId, gameId, score) VALUES ((Select Players.id FROM Players WHERE name='@playerOne'),@gameID, 1);
		 SET @secondWords:=LAST_INSERT_ID();
		 *LOOPS*
		 INSERT INTO Words (id, word, isValid) VALUES (@firstWords, '@WORD1', 1);
		 INSERT INTO Words (id, word, isValid) VALUES (@secondWords, '@WORD2', 1);
		 *END LOOPS*

		 This query is a quite simple mix of INSERTS and variable storing.
		 The first thing is that the players are added into the Players table if they don't already exist.
		 A new game row is inserted into the Games table and its unique Id is stored to a variable.
		 For each of the two players, a new Players_Games row is created and the generated WordsId is stored to a variable.
		 Finally for each word used within the game, a loop of INSERTs is used to insert each word corresponding to each player and their WordID.

FIN.