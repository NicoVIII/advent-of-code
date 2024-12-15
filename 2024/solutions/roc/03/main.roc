app [main] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.17.0/lZFLstMUCUvd5bjnnpYromZJXkQUrdhbva4xdBInicE.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

Data : Str

parse1 : Data -> _
parse1 = \data ->
    Str.splitOn data "mul("
    |> List.dropFirst 1 # First part is never part of a valid pattern
    |> List.keepOks
        (\line ->
            paramStrResult =
                Str.splitOn line ")"
                |> List.get 0
            when paramStrResult is
                Ok paramStr ->
                    params =
                        paramStr
                        |> Str.splitOn ","
                        |> List.map Str.toU32
                    when params is
                        [Ok param1, Ok param2] -> Ok (param1, param2)
                        _ -> Err {}

                Err _ -> Err {}
        )

expect parse1 "mul(1,2)mul(3,4)" == [(1, 2), (3, 4)]
expect parse1 "mul( 1,2 )mul(3,4)mul(#5,6+)" == [(3, 4)]

part1 : Data -> _
part1 = \data ->
    parse1 data
    |> List.map (\(param1, param2) -> param1 * param2)
    |> List.sum

# TODO - look if I can use IO in expect with PI

main =
    args = Arg.list! {}
    when List.get args 1 is
        Ok file ->
            data = File.readUtf8! file
            Stdout.line! "Part1: $(part1 data |> Num.toStr)"

        # Stdout.line! "Part2: $(part2 data |> Num.toStr)"
        Err _ ->
            crash "Failed to read file"
