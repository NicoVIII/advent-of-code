app [main!] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.19.0/Hj-J_zxz7V9YurCSTFcFdu6cQJie4guzsPMUi5kBYUk.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

Level : U8
Report : List Level
Data : List Report

# Because there is no List.windowed, we build our own
list_windowed2 : List a -> List (a, a)
list_windowed2 = |list|
    List.drop_last(list, 1)
    |> List.walk_with_index(
        [],
        |windows, element, index|
            when List.get(list, (index + 1)) is
                Ok(next) -> List.append(windows, (element, next))
                Err(_) -> crash("Somewhing went wrong creating windowed list :/"),
    )

expect list_windowed2([1, 2, 3]) == [(1, 2), (2, 3)]
expect list_windowed2([1, 2, 3, 4, 5]) == [(1, 2), (2, 3), (3, 4), (4, 5)]

parse : Str -> Data
parse = |input|
    parse_line = |line|
        Str.split_on(line, " ")
        |> List.map(
            |level|
                when Str.to_u8(level) is
                    Ok(value) -> value
                    Err(_) -> crash("Failed to parse level: Invalid number ${level}"),
        )

    Str.split_on(input, "\n")
    |> List.keep_if(|line| line != "")
    |> List.map(parse_line)

expect parse("1 2 3") == [[1, 2, 3]]
expect parse("1 2 3\n4 5 6\n") == [[1, 2, 3], [4, 5, 6]]

is_report_safe : Report -> Bool
is_report_safe = |report|
    all_increasing = report == List.sort_asc(report)
    all_decreasing = report == List.sort_desc(report)
    changes_gradually =
        list_windowed2(report)
        |> List.all(
            |(a, b)|
                diff = Num.abs_diff(a, b)
                diff >= 1 and diff <= 3,
        )
    (all_increasing or all_decreasing) and changes_gradually

part1 : Data -> _
part1 = |data| List.count_if(data, is_report_safe)

# TODO - look if I can use IO in expect with PI

part2 : Data -> _
part2 = |data|
    create_problem_dampener_list : Report -> List Report
    create_problem_dampener_list = |report|
        List.walk_with_index(
            report,
            [],
            |reduced_list, _, index|
                List.append(reduced_list, List.drop_at(report, index)),
        )

    List.count_if(
        data,
        |report|
            List.any(
                create_problem_dampener_list(report),
                is_report_safe,
            ),
    )

run! = |args|
    file = try(List.get, args, 1)
    input = try(File.read_utf8!, file)
    data = parse(input)
    try(Stdout.line!, "Part1: ${part1(data) |> Num.to_str}")
    try(Stdout.line!, "Part2: ${part2(data) |> Num.to_str}")
    Ok({})

main! = |raw_args|
    args = List.map(raw_args, Arg.display)
    run!(args)
