![icon](/docs/icon.png)

# setlistbot

A setlistbot for reddit and discord.

## Reddit

[/u/setlistbot](https://reddit.com/user/setlistbot) is a bot that posts setlists to reddit. It can be used in the following subreddits:

- [/r/phish](https://reddit.com/r/phish)
- [/r/gratefuldead](https://reddit.com/r/gratefuldead)

Each subreddit supports only a single artist.

- Phish
- Grateful Dead
<!-- - King Gizzard & the Lizard Wizard -->

## Discord

You can add the bot to your discord server by using this link:

[Add setlistbot to Discord](https://discord.com/api/oauth2/authorize?client_id=1091917839209869413&permissions=2048&scope=bot)

![discord](/docs/discord-example.png)

Supported commands:

`/setlist <artist> <date>`

Supported artists:

- Phish
- Grateful Dead
- King Gizzard & the Lizard Wizard

# Design

## System Context

![System Context](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c1-system-context.puml)

## Container

![Container](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c2-container.puml)

## Component

![Component](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c3-component.puml)

## Class

[Reddit Bot](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c4-class-reddit-bot.puml)

[Phish Provider](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c4-class-phish-provider.puml)

[Grateful Dead Provider](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c4-class-grateful-dead-provider.puml)

[King Gizzard & the Lizard Wizard Provider](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/cjbanna/setlistbot/main/docs/setlistbot-c4-class-kglw-provider.puml)
