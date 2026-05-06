type HeroProps = {
  title: string;
  message: string;
};

function Hero({ title, message }: HeroProps) {
  return (
    <section className="hero">
      <h2>{title}</h2>
      <p>{message}</p>
    </section>
  );
}

export default Hero;