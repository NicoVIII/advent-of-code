# README

To create a new day, run `gleam new day<day>`. After that delete the .github folder and the README.
For testing purposes add testbase to your dev dependencies and call it in one of your tests. You will
also always need `argv` for argument parsing and `simplifile` for file reading. You can add those by
cd-ing into the day folder and run `gleam add argv simplifile`.
