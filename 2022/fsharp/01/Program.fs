open System
open System.IO

[<Measure>]
type cal

type Elf = {
    calories: uint<cal> list
}

// Read input file
File.ReadLines "input.txt"
// Transform input into a useable form
|> Seq.fold(fun (elfes, calories) line ->
    match line.Trim() with
    | "" ->
        let elf = { calories = calories }
        (Seq.append elfes [elf], [])
    | line ->
        let calories_line = (UInt32.Parse line) * 1u<cal>
        (elfes, calories_line :: calories)
) (Seq.empty, [])
// Finish collection of last elf
|> fun (elfes, calories) ->
    match calories with
    | [] -> elfes
    | calories -> 
        let elf = { calories = calories }
        Seq.append elfes [elf]

// Sum up calories per elf 
|> Seq.map (fun elf -> elf.calories |> Seq.sum)
// Sum up top 3 calories
|> Seq.sortDescending // For one elf you could use maxBy instead of sortDescending >> take
|> Seq.take 3 // Change this to 1 for part 1
|> Seq.sum
|> printfn "Calories: %i"
