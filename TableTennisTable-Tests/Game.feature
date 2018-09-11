Feature: Game

    Scenario: Empty League
	    Given the league has no players
	    When I print the league
	    Then I should see "No players yet"

    Scenario: Adding Players
        Given the league has players
          | Player  |
          | Alice   |
          | Bob     |
          | Charles |
          | Dana    |
        When I print the league
        Then I should see "Alice" in row 1
        And I should see "Bob" in row 2
        And I should see "Charles" in row 2
        And I should see "Dana" in row 3

    Scenario: Find the winner
        Given the league has players
          | Player  |
          | Alice   |
          | Bob     |
          | Charles |
          | Dana    |
        When I check the winner
        Then I should see "Alice"

    Scenario: Record a win
        Given the league has players
          | Player  |
          | Alice   |
          | Bob     |
          | Charles |
        When "Charles" wins a game against "Alice"
        And I print the league
        Then I should see "Charles" in row 1
        And I should see "Alice" in row 2

    Scenario: Illegal win
        Given the league has players
          | Player  |
          | Alice   |
          | Bob     |
          | Charles |
          | Dana    |
        When "Dana" wins a game against "Alice"
        Then I should see "Cannot record match result. Winner Dana must be one row below loser Alice"
        When I print the league
        Then I should see "Alice" in row 1
        And I should see "Dana" in row 3

    Scenario: Save and Load
        Given the league has players
          | Player  |
          | Alice   |
          | Bob     |
          | Charles |
          | Dana    |
        When I save the game to "my_save"
        And I reset the game
        And I load the game from "my_save"
        And I print the league
        Then I should see "Alice" in row 1
        And I should see "Bob" in row 2
        And I should see "Charles" in row 2
        And I should see "Dana" in row 3
