# WipeCountdown

This basic plugin provides a countdown and date of the next recurring wipe in chat or console.

## Installation

Simply install in the `~/oxide/plugins` directory and enjoy.

## Configuration

The plugin will create a configuration file by default

```json
{
  "WipeCycle": "weekly",
  "WipeDay": "friday",
  "WipeTimeHourUTC": 14,
  "WipeTimeMinuteUTC": 0
}
```

| Option              | Description                                                | Values                                                                 |
| ------------------- | ---------------------------------------------------------- | ---------------------------------------------------------------------- |
| `WipeCycle`         | The recurring cycle of the wipe                            | `weekly` `biweekly` `monthly`                                          |
| `WipeDay`           | The week day of the wipe (must be lowercase)               | `sunday` `monday` `tuesday` `wednesday` `thursday` `friday` `saturday` |
| `WipeTimeHourUTC`   | The hour time of the wipe in UTC timezone (24-hour format) | `0-24`                                                                 |
| `WipeTimeMinuteUTC` | The minute time of the wipe                                | `0-59`                                                                 |

## Permissions

This plugin does not use permissions
