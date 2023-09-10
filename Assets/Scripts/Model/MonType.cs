using System.Xml.Serialization;
public enum MON
{
   [XmlEnum("MON.ERROR")]
   ERROR,
    [XmlEnum("MON.ACOLYTE")]
    ACOLYTE,
    [XmlEnum("MON.APE_WHITE")]
    APE_WHITE,
    [XmlEnum("MON.BANDIT")]
    BANDIT,
    [XmlEnum("MON.BASILISK")]
    BASILISK,
    [XmlEnum("MON.BAT")]
    BAT,
    [XmlEnum("MON.BAT_GIANT")]
    BAT_GIANT,
    [XmlEnum("MON.BAT_VAMPIRE")]
    BAT_VAMPIRE,
    [XmlEnum("MON.BEAR_BLACK")]
    BEAR_BLACK,
    [XmlEnum("MON.BEAR_CAVE")]
    BEAR_CAVE,
    [XmlEnum("MON.BEAR_GRIZZLY")]
    BEAR_GRIZZLY,
    [XmlEnum("MON.BEAR_POLAR")]
    BEAR_POLAR,
    [XmlEnum("MON.BEETLE_FIRE")]
    BEETLE_FIRE,
    [XmlEnum("MON.BEETLE_OIL")]
    BEETLE_OIL,
    [XmlEnum("MON.BEETLE_TIGER")]
    BEETLE_TIGER,
    [XmlEnum("MON.BERSERKER")]
    BERSERKER,
    [XmlEnum("MON.BLACK_PUDDING")]
    BLACK_PUDDING,
    [XmlEnum("MON.BLINK_DOG")]
    BLINK_DOG,
    [XmlEnum("MON.BOAR")]
    BOAR,
    [XmlEnum("MON.BRIGAND")]
    BRIGAND,
    [XmlEnum("MON.BUCCANEER")]
    BUCCANEER,
    [XmlEnum("MON.BUGBEAR")]
    BUGBEAR,
    [XmlEnum("MON.CAECILIA")]
    CAECILIA,
    [XmlEnum("MON.CAMEL")]
    CAMEL,
    [XmlEnum("MON.CARCASS_CRAWLER")]
    CARCASS_CRAWLER,
    [XmlEnum("MON.CAT_LION")]
    CAT_LION,
    [XmlEnum("MON.CAT_MOUNTAIN_LION")]
    CAT_MOUNTAIN_LION,
    [XmlEnum("MON.CAT_PANTHER")]
    CAT_PANTHER,
    [XmlEnum("MON.SABRETOOTH_TIGER")]
    CAT_SABRETOOTH_TIGER,
    [XmlEnum("MON.TIGER")]
    CAT_TIGER,
    [XmlEnum("MON.CAVE_LOCUST")]
    CAVE_LOCUST,
    [XmlEnum("MON.CENTUAR")]
    CENTUAR,
    [XmlEnum("MON.CENTIPEDE_GIANT")]
    CENTIPEDE_GIANT,
    [XmlEnum("MON.CHIMERA")]
    CHIMERA,
    [XmlEnum("MON.COCKATRICE")]
    COCKATRICE,
    [XmlEnum("MON.CRAB_GIANT")]
    CRAB_GIANT,
    [XmlEnum("MON.CROCODILE")]
    CROCODILE,
    [XmlEnum("MON.CYCLOPS")]
    CYCLOPS,
    [XmlEnum("MON.DERVISH")]
    DERVISH,
    [XmlEnum("MON.DJINNI_LESSER")]
    DJINNI_LESSER,
    [XmlEnum("MON.DOPPELGANGER")]
    DOPPELGANGER,
    [XmlEnum("MON.DRAGON_BLACK")]
    DRAGON_BLACK,
    [XmlEnum("MON.DRAGON_BLUE")]
    DRAGON_BLUE,
    [XmlEnum("MON.DRAGON_GOLD")]
    DRAGON_GOLD,
    [XmlEnum("MON.DRAGON_GREEN")]
    DRAGON_GREEN,
    [XmlEnum("MON.DRAGON_RED")]
    DRAGON_RED,
    [XmlEnum("MON.DRAGON_SEA")]
    DRAGON_SEA,
    [XmlEnum("MON.DRAGON_WHITE")]
    DRAGON_WHITE,
    [XmlEnum("MON.DRAGON_TURTLE")]
    DRAGON_TURTLE,
    [XmlEnum("MON.DRIVER_ANT")]
    DRIVER_ANT,
    [XmlEnum("MON.DRYAD")]
    DRYAD,
    [XmlEnum("MON.DWARF")]
    DWARF,
    [XmlEnum("MON.EFREETI_LESSER")]
    EFREETI_LESSER,
    [XmlEnum("MON.ELEMENTAL_AIR")]
    ELEMENTAL_AIR,
    [XmlEnum("MON.ELEMENTAL_EARTH")]
    ELEMENTAL_EARTH,
    [XmlEnum("MON.ELEMENTAL_WATER")]
    ELEMENTAL_WATER,
    [XmlEnum("MON.ELEMENTAL_FIRE")]
    ELEMENTAL_FIRE,
    [XmlEnum("MON.ELEPHANT")]
    ELEPHANT,
    [XmlEnum("MON.ELF")]
    ELF,
    [XmlEnum("MON.FERRET_GIANT")]
    FERRET_GIANT,
    [XmlEnum("MON.FISH_BASS")]
    FISH_BASS,
    [XmlEnum("MON.FISH_PIRANHA")]
    FISH_PIRANHA,
    [XmlEnum("MON.FISH_ROCK")]
    FISH_ROCK,
    [XmlEnum("MON.STURGEON")]
    FISH_STURGEON,
    [XmlEnum("MON.GARGOYLE")]
    GARGOYLE,
    [XmlEnum("MON.GELATINOUS_CUBE")]
    GELATINOUS_CUBE,
    [XmlEnum("MON.GHOUL")]
    GHOUL,
    [XmlEnum("MON.GIANT_CLOUD")]
    GIANT_CLOUD,
    [XmlEnum("MON.FIRE")]
    GIANT_FIRE,
    [XmlEnum("MON.FROST")]
    GIANT_FROST,
    [XmlEnum("MON.HILL")]
    GIANT_HILL,
    [XmlEnum("MON.STONE")]
    GIANT_STONE,
    [XmlEnum("MON.STORM")]
    GIANT_STORM,
    [XmlEnum("MON.GNOLL")]
    GNOLL,
    [XmlEnum("MON.GNOME")]
    GNOME,
    [XmlEnum("MON.GOBLIN")]
    GOBLIN,
    [XmlEnum("MON.GOLEM_AMBER")]
    GOLEM_AMBER,
    [XmlEnum("MON.GOLEM_BONE")]
    GOLEM_BONE,
    [XmlEnum("MON.GOLEM_BRONZE")]
    GOLEM_BRONZE,
    [XmlEnum("MON.GOLEM_WOOD")]
    GOLEM_WOOD,
    [XmlEnum("MON.GORGON")]
    GORGON,
    [XmlEnum("MON.GREY_OOZE")]
    GREY_OOZE,
    [XmlEnum("MON.GREEN_SLIME")]
    GREEN_SLIME,
    [XmlEnum("MON.GRIFFON")]
    GRIFFON,
    [XmlEnum("MON.HALFLING")]
    HALFLING,
    [XmlEnum("MON.HARPY")]
    HARPY,
    [XmlEnum("MON.HAWK")]
    HAWK,
    [XmlEnum("MON.HELLHOUND")]
    HELLHOUND,
    [XmlEnum("MON.HERD_ANIMAL_LARGE")]
    HERD_ANIMAL_LARGE, // Elk, moose
    [XmlEnum("MON.HERD_ANIMAL_MEDIUM")]
    HERD_ANIMAL_MEDIUM, // Caribou, oxen
    [XmlEnum("MON.HERD_ANIMAL_SMALL")]
    HERD_ANIMAL_SMALL,// Antelope, deer, goats,
    [XmlEnum("MON.HIPPOGRIFF")]
    HIPPOGRIFF,
    [XmlEnum("MON.HOBGOBLIN")]
    HOBGOBLIN,
    [XmlEnum("MON.HORSE_DRAFT")]
    HORSE_DRAFT,
    [XmlEnum("MON.HORSE_RIDING")]
    HORSE_RIDING,
    [XmlEnum("MON.HORSE_WAR")]
    HORSE_WAR,
    [XmlEnum("MON.HORSE_WILD")]
    HORSE_WILD,
    [XmlEnum("MON.HYDRA")]
    HYDRA,
    [XmlEnum("MON.INSECT_SWARM")]
    INSECT_SWARM,
    [XmlEnum("MON.INVISIBLE_STALKER")]
    INVISIBLE_STALKER,
    [XmlEnum("MON.KILLER_BEE")]
    KILLER_BEE,
    [XmlEnum("MON.KOBOLD")]
    KOBOLD,
    [XmlEnum("MON.LEECH_GIANT")]
    LEECH_GIANT,
    [XmlEnum("MON.LIVING_STATUE_CRYTAL")]
    LIVING_STATUE_CRYSTAL,
    [XmlEnum("MON.LIVING_STATUE_IRON")]
    LIVING_STATUE_IRON,
    [XmlEnum("MON.LIVING_STATUE_ROCK")]
    LIVING_STATUE_ROCK,
    [XmlEnum("MON.LIZARD_DRACO")]
    LIZARD_DRACO,
    [XmlEnum("MON.LIZARD_GECKO")]
    LIZARD_GECKO,
    [XmlEnum("MON.LIZARD_CHAMELEON")]
    LIZARD_CHAMELEON,
    [XmlEnum("MON.LIZARD_TUATARA")]
    LIZARD_TUATARA,
    [XmlEnum("MON.LIZARD_MAN")]
    LIZARD_MAN,
    [XmlEnum("MON.LYCAN_SWINE")]
    LYCAN_SWINE,
    [XmlEnum("MON.LYCAN_BEAR")]
    LYCAN_BEAR,
    [XmlEnum("MON.LYCAN_BOAR")]
    LYCAN_BOAR,
    [XmlEnum("MON.LYCAN_RAT")]
    LYCAN_RAT,
    [XmlEnum("MON.LYCAN_WOLF")]
    LYCAN_WOLF,
    [XmlEnum("MON.MANTICORE")]
    MANTICORE,
    [XmlEnum("MON.MASTODON")]
    MASTODON,
    [XmlEnum("MON.MEDIUM")]
    MEDIUM,
    [XmlEnum("MON.MEDUSA")]
    MEDUSA,
    [XmlEnum("MON.MERCHANT")]
    MERCHANT,
    [XmlEnum("MON.MERMAN")]
    MERMAN,
    [XmlEnum("MON.MINOTAUR")]
    MINOTAUR,
    [XmlEnum("MON.MULE")]
    MULE,
    [XmlEnum("MON.MUMMY")]
    MUMMY,
    [XmlEnum("MON.NEANDERTHAL")]
    NEANDERTHAL,
    [XmlEnum("MON.NIXIE")]
    NIXIE,
    [XmlEnum("MON.NOBLE")]
    NOBLE,
    [XmlEnum("MON.NOMAD")]
    NOMAD,
    [XmlEnum("MON.NORMAL_HUMAN")]
    NORMAL_HUMAN,
    [XmlEnum("MON.OCHRE_JELLY")]
    OCHRE_JELLY,
    [XmlEnum("MON.OCTOPUS_GIANT")]
    OCTOPUS_GIANT,
    [XmlEnum("MON.OGRE")]
    OGRE,
    [XmlEnum("MON.ORC")]
    ORC,
    [XmlEnum("MON.OWLBEAR")]
    OWLBEAR,
    [XmlEnum("MON.PEGASUS")]
    PEGASUS,
    [XmlEnum("MON.PIRATE")]
    PIRATE,
    [XmlEnum("MON.PIXIE")]
    PIXIE,
    [XmlEnum("MON.PTEROSAUR")]
    PTEROSAUR,
    [XmlEnum("MON.PURPLE_WORM")]
    PURPLE_WORM,
    [XmlEnum("MON.RAT_GIANT")]
    RAT_GIANT,
    [XmlEnum("MON.RAT_NORMAL")]
    RAT_NORMAL,
    [XmlEnum("MON.RHAGODESSA")]
    RHAGODESSA,
    [XmlEnum("MON.RHINOCEROS")]
    RHINOCEROS,
    [XmlEnum("MON.ROBBER_FLY")]
    ROBBER_FLY,
    [XmlEnum("MON.ROC")]
    ROC,
    [XmlEnum("MON.ROCK_BABOON")]
    ROCK_BABOON,
    [XmlEnum("MON.RUST_MONSTER")]
    RUST_MONSTER,
    [XmlEnum("MON.SALAMANDER_FLAME")]
    SALAMANDER_FLAME,
    [XmlEnum("MON.SALAMANDER_FROST")]
    SALAMANDER_FROST,
    [XmlEnum("MON.SCORPION_GIANT")]
    SCORPION_GIANT,
    [XmlEnum("MON.SEA_SERPENT")]
    SEA_SERPENT,
    [XmlEnum("MON.SHADOW")]
    SHADOW,
    [XmlEnum("MON.SHARK_BULL")]
    SHARK_BULL,
    [XmlEnum("MON.SHARK_GREATER")]
    SHARK_GREATER,
    [XmlEnum("MON.SHARK_MAKO")]
    SHARK_MAKO,
    [XmlEnum("MON.SHREW_GIANT")]
    SHREW_GIANT,
    [XmlEnum("MON.SHRIEKER")]
    SHRIEKER,
    [XmlEnum("MON.SKELETON")]
    SKELETON,
    [XmlEnum("MON.SNAKE_RATTLER")]
    SNAKE_RATTLER,
    [XmlEnum("MON.SNAKE_VIPER")]
    SNAKE_VIPER,
    [XmlEnum("MON.SNAKE_PYTHON")]
    SNAKE_PYTHON,
    [XmlEnum("MON.SNAKE_SEA")]
    SNAKE_SEA,
    [XmlEnum("MON.SNAKE_COBRA")]
    SNAKE_COBRA,
    [XmlEnum("MON.SPECTRE")]
    SPECTRE,
    [XmlEnum("MON.SPIDER_BLACK_WIDOW")]
    SPIDER_BLACK_WIDOW,
    [XmlEnum("MON.SPIDER_CRAB")]
    SPIDER_CRAB,
    [XmlEnum("MON.SPIDER_TARANTELLA")]
    SPIDER_TARANTELLA,
    [XmlEnum("MON.SPRITE")]
    SPRITE,
    [XmlEnum("MON.SQUID_GIANT")]
    SQUID_GIANT,
    [XmlEnum("MON.STEGOSAURUS")]
    STEOGSAURUS,
    [XmlEnum("MON.STIRGE")]
    STIRGE,
    [XmlEnum("MON.THOUL")]
    THOUL,
    [XmlEnum("MON.TITANOTHERE")]
    TITANOTHERE,
    [XmlEnum("MON.TOAD_GIANT")]
    TOAD_GIANT,
    [XmlEnum("MON.TRADER")]
    TRADER,
    [XmlEnum("MON.TREANT")]
    TREANT,
    [XmlEnum("MON.TRICERATOPS")]
    TRICERATOPS,
    [XmlEnum("MON.TROGLODYTE")]
    TROGLODYTE,
    [XmlEnum("MON.TROLL")]
    TROLL,
    [XmlEnum("MON.TREX")]
    TREX,
    [XmlEnum("MON.UNICORN")]
    UNICORN,
    [XmlEnum("MON.VAMPIRE")]
    VAMPIRE,
    [XmlEnum("MON.VETERAN")]
    VETERAN,
    [XmlEnum("MON.WARP_BEAST")]
    WARP_BEAST,
    [XmlEnum("MON.WATER_TERMITE")]
    WATER_TERMITE,
    [XmlEnum("MON.WEASEL_GIANT")]
    WEASEL_GIANT,
    [XmlEnum("MON.WHALE_KILLER")]
    WHALE_KILLER,
    [XmlEnum("MON.WHALE_NARWHAL")]
    WHALE_NARWHAL,
    [XmlEnum("MON.WHALE_SPERM")]
    WHALE_SPERM,
    [XmlEnum("MON.WHITE")]
    WIGHT,
    [XmlEnum("MON.WOLF_DIRE")]
    WOLF_DIRE,
    [XmlEnum("MON.WOLF_NORMAL")]
    WOLF_NORMAL,
    [XmlEnum("MON.WRAITH")]
    WRAITH,
    [XmlEnum("MON.WYVERN")]
    WYVERN,
    [XmlEnum("MON.YELLOW_MOULD")]
    YELLOW_MOULD,
    [XmlEnum("MON.ZOMBIE")]
    ZOMBIE
}
