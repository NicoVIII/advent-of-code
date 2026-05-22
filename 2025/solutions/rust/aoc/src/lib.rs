use std::env;
use std::fs;
use std::path::Path;

pub fn run_cli<Parsed, Parse, SolvePart1, SolvePart2>(
    parse: Parse,
    solve_part1: SolvePart1,
    solve_part2: SolvePart2,
) -> Result<(), String>
where
    Parse: Fn(&str) -> Result<Parsed, String>,
    SolvePart1: Fn(&Parsed) -> String,
    SolvePart2: Fn(&Parsed) -> String,
{
    let input_path = env::args()
        .nth(1)
        .ok_or_else(|| "expected input file path as the first argument".to_string())?;
    let input = fs::read_to_string(&input_path)
        .map_err(|error| format!("failed to read {input_path}: {error}"))?;
    let parsed = parse(input.trim())?;

    println!("Part 1: {}", solve_part1(&parsed));
    println!("Part 2: {}", solve_part2(&parsed));

    Ok(())
}

pub fn run_for_testset<Parsed, Parse, SolvePart1, SolvePart2>(
    manifest_dir: &Path,
    day: &str,
    parse: Parse,
    solve_part1: Option<SolvePart1>,
    solve_part2: Option<SolvePart2>,
) -> Result<(), String>
where
    Parse: Fn(&str) -> Result<Parsed, String>,
    SolvePart1: Fn(&Parsed) -> String,
    SolvePart2: Fn(&Parsed) -> String,
{
    if solve_part1.is_none() && solve_part2.is_none() {
        return Err("No parts to test".to_string());
    }

    let base_path = manifest_dir.join("../../../puzzles").join(day);
    let mut entries = fs::read_dir(&base_path)
        .map_err(|error| format!("failed to read {}: {error}", base_path.display()))?
        .map(|entry| {
            entry
                .map_err(|error| format!("failed to read directory entry: {error}"))
                .map(|entry| entry.file_name().to_string_lossy().into_owned())
        })
        .collect::<Result<Vec<_>, _>>()?;
    entries.sort();

    for input_name in entries
        .into_iter()
        .filter(|entry| entry.ends_with("-input.txt"))
    {
        let input_path = base_path.join(&input_name);
        let output_path = base_path.join(output_filename(&input_name));
        let input = fs::read_to_string(&input_path)
            .map_err(|error| format!("failed to read {}: {error}", input_path.display()))?;
        let output = fs::read_to_string(&output_path)
            .map_err(|error| format!("failed to read {}: {error}", output_path.display()))?;
        let (part1_expected, part2_expected) = parse_expected_output(&output)?;

        if part1_expected.is_none() && part2_expected.is_none() {
            return Err(format!(
                "No expected results to test against in {}",
                output_path.display()
            ));
        }

        let parsed = parse(input.trim())?;

        if let (Some(solve_part1), Some(expected)) = (&solve_part1, &part1_expected) {
            let actual = solve_part1(&parsed);
            if actual != *expected {
                return Err(format!(
                    "Part 1 mismatch for {}: expected {}, got {}",
                    input_path.display(),
                    expected,
                    actual
                ));
            }
        }

        if let (Some(solve_part2), Some(expected)) = (&solve_part2, &part2_expected) {
            let actual = solve_part2(&parsed);
            if actual != *expected {
                return Err(format!(
                    "Part 2 mismatch for {}: expected {}, got {}",
                    input_path.display(),
                    expected,
                    actual
                ));
            }
        }
    }

    Ok(())
}

fn output_filename(input_name: &str) -> String {
    input_name.replace("-input", "-output")
}

fn parse_expected_output(output: &str) -> Result<(Option<String>, Option<String>), String> {
    let lines = output
        .trim()
        .lines()
        .map(str::trim)
        .map(parse_expected_value)
        .collect::<Vec<_>>();

    if lines.len() != 2 {
        return Err(format!(
            "expected exactly 2 output lines, got {}",
            lines.len()
        ));
    }

    Ok((lines[0].clone(), lines[1].clone()))
}

fn parse_expected_value(line: &str) -> Option<String> {
    if line == "-" {
        None
    } else {
        Some(line.to_string())
    }
}