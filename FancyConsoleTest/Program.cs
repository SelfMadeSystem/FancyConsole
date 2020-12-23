using System;
using System.Threading;
using FancyConsoleTest.New;
using FancyConsoleTest.New.Utils;

namespace FancyConsoleTest {
    class Program {
        static void Main(string[] args) {
            // try {
                GuiApp.Log(new FancyText("Black ", FancyColor.Black) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple) {
                                        Next = new FancyText("Gold ", FancyColor.Gold) {
                                            Next = new FancyText("Gray ", FancyColor.Gray) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue) {
                                                        Next = new FancyText("Green ", FancyColor.Green) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua) {
                                                                Next = new FancyText("Red ", FancyColor.Red) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow) {
                                                                            Next = new FancyText("White ", FancyColor.White)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Log(new FancyText("Black ", FancyColor.Black, FancyColor.Bold) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue, FancyColor.Bold) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen, FancyColor.Bold) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua, FancyColor.Bold) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed, FancyColor.Bold) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple, FancyColor.Bold) {
                                        Next = new FancyText("Gold ", FancyColor.Gold, FancyColor.Bold) {
                                            Next = new FancyText("Gray ", FancyColor.Gray, FancyColor.Bold) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray, FancyColor.Bold) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue, FancyColor.Bold) {
                                                        Next = new FancyText("Green ", FancyColor.Green, FancyColor.Bold) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua, FancyColor.Bold) {
                                                                Next = new FancyText("Red ", FancyColor.Red, FancyColor.Bold) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple, FancyColor.Bold) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow, FancyColor.Bold) {
                                                                            Next = new FancyText("White Bold", FancyColor.White, FancyColor.Bold)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Log(new FancyText("Black ", FancyColor.Black, FancyColor.Italic) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue, FancyColor.Italic) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen, FancyColor.Italic) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua, FancyColor.Italic) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed, FancyColor.Italic) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple, FancyColor.Italic) {
                                        Next = new FancyText("Gold ", FancyColor.Gold, FancyColor.Italic) {
                                            Next = new FancyText("Gray ", FancyColor.Gray, FancyColor.Italic) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray, FancyColor.Italic) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue, FancyColor.Italic) {
                                                        Next = new FancyText("Green ", FancyColor.Green, FancyColor.Italic) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua, FancyColor.Italic) {
                                                                Next = new FancyText("Red ", FancyColor.Red, FancyColor.Italic) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple, FancyColor.Italic) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow, FancyColor.Italic) {
                                                                            Next = new FancyText("White Italic", FancyColor.White, FancyColor.Italic)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Log(new FancyText("Black ", FancyColor.Black, FancyColor.Underline) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue, FancyColor.Underline) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen, FancyColor.Underline) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua, FancyColor.Underline) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed, FancyColor.Underline) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple, FancyColor.Underline) {
                                        Next = new FancyText("Gold ", FancyColor.Gold, FancyColor.Underline) {
                                            Next = new FancyText("Gray ", FancyColor.Gray, FancyColor.Underline) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray, FancyColor.Underline) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue, FancyColor.Underline) {
                                                        Next = new FancyText("Green ", FancyColor.Green, FancyColor.Underline) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua, FancyColor.Underline) {
                                                                Next = new FancyText("Red ", FancyColor.Red, FancyColor.Underline) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple, FancyColor.Underline) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow, FancyColor.Underline) {
                                                                            Next = new FancyText("White Underline", FancyColor.White, FancyColor.Underline)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Log(new FancyText("Black ", FancyColor.Black, FancyColor.Strikethrough) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue, FancyColor.Strikethrough) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen, FancyColor.Strikethrough) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua, FancyColor.Strikethrough) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed, FancyColor.Strikethrough) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple, FancyColor.Strikethrough) {
                                        Next = new FancyText("Gold ", FancyColor.Gold, FancyColor.Strikethrough) {
                                            Next = new FancyText("Gray ", FancyColor.Gray, FancyColor.Strikethrough) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray, FancyColor.Strikethrough) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue, FancyColor.Strikethrough) {
                                                        Next = new FancyText("Green ", FancyColor.Green, FancyColor.Strikethrough) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua, FancyColor.Strikethrough) {
                                                                Next = new FancyText("Red ", FancyColor.Red, FancyColor.Strikethrough) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple, FancyColor.Strikethrough) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow, FancyColor.Strikethrough) {
                                                                            Next = new FancyText("White Strikethrough", FancyColor.White, FancyColor.Strikethrough)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Log(new FancyText("Black ", FancyColor.Black, FancyColor.Obfuscated) {
                    Next = new FancyText("DarkBlue ", FancyColor.DarkBlue, FancyColor.Obfuscated) {
                        Next = new FancyText("DarkGreen ", FancyColor.DarkGreen, FancyColor.Obfuscated) {
                            Next = new FancyText("DarkAqua ", FancyColor.DarkAqua, FancyColor.Obfuscated) {
                                Next = new FancyText("DarkRed ", FancyColor.DarkRed, FancyColor.Obfuscated) {
                                    Next = new FancyText("DarkPurple ", FancyColor.DarkPurple, FancyColor.Obfuscated) {
                                        Next = new FancyText("Gold ", FancyColor.Gold, FancyColor.Obfuscated) {
                                            Next = new FancyText("Gray ", FancyColor.Gray, FancyColor.Obfuscated) {
                                                Next = new FancyText("DarkGray ", FancyColor.DarkGray, FancyColor.Obfuscated) {
                                                    Next = new FancyText("Blue ", FancyColor.Blue, FancyColor.Obfuscated) {
                                                        Next = new FancyText("Green ", FancyColor.Green, FancyColor.Obfuscated) {
                                                            Next = new FancyText("Aqua ", FancyColor.Aqua, FancyColor.Obfuscated) {
                                                                Next = new FancyText("Red ", FancyColor.Red, FancyColor.Obfuscated) {
                                                                    Next = new FancyText("Purple ", FancyColor.Purple, FancyColor.Obfuscated) {
                                                                        Next = new FancyText("Yellow ", FancyColor.Yellow, FancyColor.Obfuscated) {
                                                                            Next = new FancyText("White Obfuscated", FancyColor.White, FancyColor.Obfuscated)
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                GuiApp.Start(args);
            /*} catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }*/
        }
    }
}