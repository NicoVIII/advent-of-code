app [main] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.17.0/lZFLstMUCUvd5bjnnpYromZJXkQUrdhbva4xdBInicE.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

NumType : U32
Data : (List NumType, List NumType)

parse : Str -> Data
parse = \input ->
    toTuple : Str -> (NumType, NumType)
    toTuple = \line ->
        parts = Str.splitOn line "   "
        when parts is
            [x, y] ->
                xParsed = Str.toU32 x
                yParsed = Str.toU32 y
                when (xParsed, yParsed) is
                    (Ok xValue, Ok yValue) -> (xValue, yValue)
                    _ -> crash "Failed to parse line"

            _ -> crash "Something went wrong parsing the input"

    Str.splitOn input "\n"
    |> List.keepIf (\line -> line != "")
    |> List.map toTuple
    # Transform list of pairs to two lists
    |> List.walk
        ([], [])
        (
            \(list1, list2), (el1, el2) ->
                (
                    List.append list1 el1,
                    List.append list2 el2,
                )
        )

expect parse "1   2\n3   4\n5   6\n" == ([1, 3, 5], [2, 4, 6])

part1 : Data -> _
part1 = \(list1, list2) ->
    # Sort lists
    list1Sorted = List.sortAsc list1
    list2Sorted = List.sortAsc list2
    List.map2 list1Sorted list2Sorted Num.absDiff
    |> List.sum

# TODO - look if I can use IO in expect with PI

part2 : Data -> _
part2 = \(list1, list2) ->
    List.map
        list1
        (\x ->
            occurances = List.countIf list2 (\y -> x == y) |> Num.toU32
            x * occurances
        )
    |> List.sum

main =
    args = Arg.list! {}
    when List.get args 1 is
        Ok file ->
            input = File.readUtf8! file
            data = parse input
            Stdout.line! "Part1: $(part1 data |> Num.toStr)"
            Stdout.line! "Part2: $(part2 data |> Num.toStr)"

        Err _ ->
            crash "Failed to read file"
