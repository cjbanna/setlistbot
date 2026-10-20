![icon](/docs/icon.png)

# setlistbot

A setlistbot for reddit and discord.

## Reddit

[/u/setlistbot](https://reddit.com/user/setlistbot) is a bot that posts setlists to reddit. It can be used in the following subreddits:

- [/r/phish](https://reddit.com/r/phish)
- [/r/gratefuldead](https://reddit.com/r/gratefuldead)
- [/r/kgatlw](https://reddit.com/r/kgatlw)

Reddit is shutting down its public Data API in March 2027, so the bot is moving to Reddit's Developer Platform. The new app is in [`devvit/`](devvit/README.md).

Each subreddit supports only a single artist.

- Phish
- Grateful Dead
- King Gizzard & the Lizard Wizard

### Developing the Reddit app

Requires Node 24. Run these from `devvit/`:

```sh
npm install
npm test             # unit tests
npm run test:types   # typecheck
npm run test:lint    # lint
npm run dev          # playtest on your test subreddit
npm run launch       # upload and submit for public review
```

The phish.net API key is a secret, set once with `npx devvit settings set phishNetApiKey`.

The Grateful Dead data is `src/Setlistbot.Infrastructure.GratefulDead/gd-shows.json`, which the Discord bot also uses. The files in `devvit/src/testdata` are replies from the original C# bot, so these tests check that the replies haven't changed.

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
