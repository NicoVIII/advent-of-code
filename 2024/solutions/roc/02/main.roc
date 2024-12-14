app [main] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.17.0/lZFLstMUCUvd5bjnnpYromZJXkQUrdhbva4xdBInicE.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

Level : U8
Report : List Level
Data : List Report

# Because there is no List.windowed, we build out own
listWindowed2 : List a -> List (a, a)
listWindowed2 = \list ->
    List.dropLast list 1
    |> List.walkWithIndex
        []
        (\windows, element, index ->
            when List.get list (index + 1) is
                Ok next -> List.append windows (element, next)
                Err _ -> crash "Somewhing went wrong creating windowed list :/"
        )

expect listWindowed2 [1, 2, 3] == [(1, 2), (2, 3)]
expect listWindowed2 [1, 2, 3, 4, 5] == [(1, 2), (2, 3), (3, 4), (4, 5)]

parse : Str -> Data
parse = \input ->
    parseLine = \line ->
        Str.splitOn line " "
        |> List.map
            (\level ->
                when Str.toU8 level is
                    Ok value -> value
                    Err _ -> crash "Failed to parse level: Invalid number $(level)")

    Str.splitOn input "\n"
    |> List.keepIf (\line -> line != "")
    |> List.map parseLine

expect parse "1 2 3" == [[1, 2, 3]]
expect parse "1 2 3\n4 5 6\n" == [[1, 2, 3], [4, 5, 6]]

part1 : Data -> _
part1 = \data ->
    List.countIf
        data
        (\report ->
            allIncreasing = report == List.sortAsc report
            allDecreasing = report == List.sortDesc report
            changesGradually =
                listWindowed2 report
                |> List.all
                    (\(a, b) ->
                        diff = Num.absDiff a b
                        diff >= 1 && diff <= 3)
            (allIncreasing || allDecreasing) && changesGradually
        )

# TODO - look if I can use IO in expect with PI

main =
    args = Arg.list! {}
    when List.get args 1 is
        Ok file ->
            input = File.readUtf8! file
            data = parse input
            Stdout.line! "Part1: $(part1 data |> Num.toStr)"

        # Stdout.line! "Part2: $(part2 data |> Num.toStr)"
        Err _ ->
            crash "Failed to read file"
