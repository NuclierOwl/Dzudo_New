﻿using Kurs_Dzudo.Hardik.Connector.Date;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ukhasnikis_BD_Sec.Hardik.Connect;

namespace Dzudo.ViewModels;
public class GestViewModel : INotifyPropertyChanged
{
    private readonly DatabaseConnection _dbConnection;
    private List<Tatami> _tatamis;
    private List<Group> _groups;
    private IEnumerable<object> _participants;
    private IEnumerable<object> _matches;

    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Tatami> Tatamis
    {
        get => _tatamis;
        set
        {
            _tatamis = value;
            OnPropertyChanged();
        }
    }

    public List<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<object> Participants
    {
        get => _participants;
        set
        {
            _participants = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<object> Matches
    {
        get => _matches;
        set
        {
            _matches = value;
            OnPropertyChanged();
        }
    }

    public GestViewModel()
    {
        _dbConnection = new DatabaseConnection();
        LoadTatamis();
    }

    private void LoadTatamis()
    {
        Tatamis = _dbConnection.GetAllTatamis();
    }

    public void LoadGroupsForTatami(int tatamiId)
    {
        Groups = _dbConnection.GetGroupsForTatami(tatamiId);
        Participants = Groups.SelectMany(g => g.Participants).Distinct().ToList();
    }

    public void LoadMatchesForGroup(Group group)
    {
        Matches = group.Matches;
    }

    public void OnTatamiSelectionChanged(Tatami selectedTatami)
    {
        if (selectedTatami != null)
        {
            LoadGroupsForTatami(selectedTatami.Id);
        }
    }

    public void OnGroupSelectionChanged(Group selectedGroup)
    {
        if (selectedGroup != null)
        {
            LoadMatchesForGroup(selectedGroup);
        }
    }

    private Tatami _selectedTatami;
    public Tatami SelectedTatami
    {
        get => _selectedTatami;
        set
        {
            _selectedTatami = value;
            OnPropertyChanged();
            OnTatamiSelectionChanged(value);
        }
    }

    private Group _selectedGroup;
    public Group SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            OnPropertyChanged();
            OnGroupSelectionChanged(value);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}