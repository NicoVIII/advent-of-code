use std::{cmp::Ordering, collections::HashSet, fs, str::Lines};

fn read_input() -> String {
    fs::read_to_string("input.txt").expect("Should have been able to read the file")
}

#[derive(Debug)]
enum Direction {
    Left,
    Right,
    Up,
    Down,
}

#[derive(Debug)]
struct Instruction {
    direction: Direction,
    distance: u8,
}

fn parse_input(lines: Lines<'_>) -> Vec<Instruction> {
    lines
        .map(|line| {
            let mut iter = line.split_whitespace();
            let direction = match iter.next().unwrap() {
                "L" => Direction::Left,
                "R" => Direction::Right,
                "U" => Direction::Up,
                "D" => Direction::Down,
                _ => panic!("Invalid direction"),
            };
            let distance = iter.next().unwrap().parse().unwrap();
            Instruction {
                direction,
                distance,
            }
        })
        .collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
struct Point {
    x: i32,
    y: i32,
}

type Rope = Vec<Point>;

fn create_rope(knots_amount: u8) -> Rope {
    let mut knots = Vec::with_capacity(knots_amount as usize);
    for _ in 0..knots_amount {
        knots.push(Point { x: 0, y: 0 });
    }
    knots
}

fn simulate_knot_movement(parent_knot: &Point, knot: &mut Point) {
    // Check, if head and tail aren't touching anymore
    if (parent_knot.x - knot.x).abs() > 1 || (parent_knot.y - knot.y).abs() > 1 {
        // Adjust x
        match parent_knot.x.cmp(&knot.x) {
            Ordering::Greater => knot.x += 1,
            Ordering::Less => knot.x -= 1,
            Ordering::Equal => { /* noop */ }
        }

        // Adjust y
        match parent_knot.y.cmp(&knot.y) {
            Ordering::Greater => knot.y += 1,
            Ordering::Less => knot.y -= 1,
            Ordering::Equal => { /* noop */ }
        }
    }
}

fn simulate(rope: &mut Rope, instructions: &[Instruction]) -> HashSet<Point> {
    let mut tail_history = HashSet::new();
    // Add starting position
    tail_history.insert(Point {
        x: rope[0].x,
        y: rope[0].y,
    });
    for instruction in instructions {
        for _ in 0..instruction.distance {
            match instruction.direction {
                Direction::Left => rope[0].x -= 1,
                Direction::Right => rope[0].x += 1,
                Direction::Up => rope[0].y += 1,
                Direction::Down => rope[0].y -= 1,
            }
            // Simulate movement for all knots
            rope.iter_mut().fold(None, |parent_knot, knot| {
                if let Some(parent_knot) = parent_knot {
                    simulate_knot_movement(parent_knot, knot);
                }
                Some(knot)
            });
            // Add tail position to history
            tail_history.insert(Point {
                x: rope[rope.len() - 1].x,
                y: rope[rope.len() - 1].y,
            });
        }
    }
    tail_history
}

fn main() {
    let input = read_input();
    let parsed = parse_input(input.lines());

    // Part 1
    let mut rope = create_rope(2);
    let history = simulate(&mut rope, &parsed);
    println!("{}", history.len());

    // Part 2
    let mut rope = create_rope(10);
    let history = simulate(&mut rope, &parsed);
    println!("{}", history.len());
}
