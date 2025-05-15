public enum Role
{
    use rand::Rng;
    use std::cmp::Ordering;
    use std::io;
    fn main() {
        println!("Загадано число от 1 до 100. Угадай число!");
        let secret_number = rand::thread_rng().gen_range(1..=100);
        loop {
            println!("Введите ваше число.");
            let mut guess = String::new();
            io::stdin()
                .read_line(&mut guess)
                .expect("Не удалось прочитать строку");
            let guess: u32 = match guess.trim().parse() {
                Ok(num) => num,
                Err(_) => continue,
            };
            println!("Вы ввели: {guess}");
            match guess.cmp(&secret_number) {
                Ordering::Less => println!("Мое число больше!"),
                Ordering::Greater => println!("Мое число меньше!"),
                Ordering::Equal => {
                    println!("Вы угадали!");
                    break;
                }
            }
        }
    }
}
